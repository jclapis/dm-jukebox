/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using DMJukebox.Interop;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DMJukebox
{
    /// <summary>
    /// This class represents an audio track (a single file) loaded into the jukebox. This holds all of
    /// the functionality for loading, reading, and decoding the file.
    /// </summary>
    /// <remarks>
    /// So because I'm using FFmpeg as the decoder, I can technically handle files that have multiple
    /// audio streams. I don't really know why this would ever come into play though, like would a DM
    /// ever need to load a video with an english and a french soundtrack, and choose between them?
    /// Who knows. Until I find a good reason to implement multi-stream selection, this is just going
    /// to load the first one it finds.
    /// </remarks>
    public class AudioTrack : IDisposable
    {
        /// <summary>
        /// This is a constant set within wavdec.c of libavformat that defines how big a WAV / PCM
        /// packet is. Since raw streams like those don't come with frame info, the developers just
        /// arbitrarily decided to make packets 4096 bytes large - this means the number of samples
        /// per frame actually changes depending on the number of channels, bits per sample, etc.
        /// </summary>
        private const int WavSize = 4096;

        /// <summary>
        /// This is a handle to the manager for this stream, so this can notify it when playback
        /// starts and stops.
        /// </summary>
        private AudioTrackManager Manager;

        /// <summary>
        /// This is the <see cref="AVFormatContext"/> for this file.
        /// </summary>
        private readonly IntPtr FormatContextPtr;

        /// <summary>
        /// This is the <see cref="AVPacket"/> that holds encoded data read from the file.
        /// I reuse the same one for reading over and over because of performance reasons.
        /// </summary>
        private IntPtr PacketPtr;

        /// <summary>
        /// This is the <see cref="AVFrame"/> that gets read from the data in AVPacket. It holds raw
        /// decoded data. This data doesn't get passed to the playback buffer, this is just an
        /// intermediate holder.
        /// </summary>
        private IntPtr InputFramePtr;

        /// <summary>
        /// This is the <see cref="AVFrame"/> that gets converted data from the swresample context
        /// after it converts the input frame into the Discord-friendly format. It holds the raw
        /// data that will be sent to the playback buffers.
        /// </summary>
        private IntPtr OutputFramePtr;

        /// <summary>
        /// This is the swresample context that converts data from whatever format it comes in
        /// natively into the Discord format.
        /// </summary>
        private IntPtr SwrContextPtr;

        /// <summary>
        /// This is the AVStream describing the selected audio stream from the input file.
        /// </summary>
        private readonly AVStream Stream;

        /// <summary>
        /// This is the channel layout that describes the input. Compressed audio comes with this
        /// information built into the codec so this variable doesn't get used in those cases; this
        /// only comes into play with raw (WAV / PCM) tracks that don't describe the format, because
        /// the SwrContext needs to know what format the incoming data is in before it can convert
        /// it to Discord format and we have to set it manually on each of those input frames.
        /// </summary>
        private readonly AV_CH_LAYOUT ChannelLayout;

        /// <summary>
        /// This buffer holds the decoded and converted data from the file, ready for playback.
        /// </summary>
        private readonly DecodedAudioBuffer Buffer;

        /// <summary>
        /// This is just a backing field for the Volume property.
        /// </summary>
        private float _Volume;

        /// <summary>
        /// The amount of decoded data in the buffer that's ready for playback.
        /// </summary>
        internal int AvailableData
        {
            get
            {
                return Buffer.AvailableData;
            }
        }

        /// <summary>
        /// This is the name of the track. It defaults to the file name, but you can set it to whatever
        /// you want. It's just used for display purposes so you know what track it is.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The volume of this track. You can set this to any value between
        /// 0.0 (muted) and 1.0 (full volume).
        /// </summary>
        public float Volume
        {
            get
            {
                return _Volume;
            }
            set
            {
                // Clamp it so it's between 0.0 and 1.0
                _Volume = Math.Min(1.0f, Math.Max(0.0f, value));
            }
        }

        /// <summary>
        /// This controls whether playback looping is enabled for this track. Set it to true to enable
        /// looping (so the track will start over from the beginning when the file ends), or false to
        /// disable looping (so when the file ends, playback of this track stops).
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// This describes some of the details about the track for your information. It doesn't have
        /// any bearing on playback, just here if you want to see what's going on inside the track.
        /// </summary>
        public TrackInfo Info { get; }

        /// <summary>
        /// This event is triggered when playback for this track stops.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Creates a new AudioTrack instance.
        /// </summary>
        /// <param name="Manager">The manager that created this track</param>
        /// <param name="FilePath">The path of the file to open</param>
        /// <param name="Name">The name to give the track</param>
        /// <param name="Volume">The playback volume for the track</param>
        /// <param name="Loop">Whether or not to enable playback looping for the track</param>
        /// <remarks>
        /// This doesn't have to be unsafe, but I do a lot of struct reading and writing and honestly I'm
        /// just too lazy to copy it back and forth from unmanaged memory every time something changes.
        /// </remarks>
        unsafe internal AudioTrack(AudioTrackManager Manager, string FilePath, string Name = null, float Volume = 1.0f, bool Loop = false)
        {
            // So first things first, let's make sure the file path is actually valid.
            if(!File.Exists(FilePath))
            {
                throw new FileNotFoundException($"\"{FilePath}\" is not a valid file; it doesn't seem to exist.");
            }

            this.Manager = Manager;
            this.Volume = Volume;
            this.Name = Name ?? Path.GetFileNameWithoutExtension(FilePath);
            this.Loop = Loop;

            // Create the FormatContext
            FormatContextPtr = AVFormatInterop.avformat_alloc_context();
            IntPtr options = IntPtr.Zero;
            AVERROR result = AVFormatInterop.avformat_open_input(ref FormatContextPtr, FilePath, IntPtr.Zero, ref options);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Opening the file failed with code {result}");
            }

            // Read the info for the streams in the file
            options = IntPtr.Zero;
            result = AVFormatInterop.avformat_find_stream_info(FormatContextPtr, ref options);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Reading the file's stream info failed with code {result}");
            }

            // Find the first audio stream
            AVFormatContext formatContext = Marshal.PtrToStructure<AVFormatContext>(FormatContextPtr);
            bool foundStream = false;
            AVCodecContext codecContext = default(AVCodecContext);
            for (int i = 0; i < formatContext.nb_streams; i++)
            {
                IntPtr streamPtr = Marshal.ReadIntPtr(formatContext.streams, IntPtr.Size * i);
                Stream = Marshal.PtrToStructure<AVStream>(streamPtr);
                codecContext = Marshal.PtrToStructure<AVCodecContext>(Stream.codec);
                if (codecContext.codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                {
                    foundStream = true;
                    break;
                }
            }
            if (!foundStream)
            {
                throw new Exception("No audio streams detected in the file.");
            }

            // Prepare the decoder
            IntPtr codecPtr = AVCodecInterop.avcodec_find_decoder(codecContext.codec_id);
            if (codecPtr == IntPtr.Zero)
            {
                throw new Exception($"Error loading audio codec: finding the decoder for codec ID {codecContext.codec_id} failed.");
            }
            AVCodec codec = Marshal.PtrToStructure<AVCodec>(codecPtr);
            options = IntPtr.Zero;
            result = AVCodecInterop.avcodec_open2(Stream.codec, codecPtr, ref options);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error loading audio codec: opening codec failed with {result}.");
            }

            // Set up the packet - no need for doing the buffers, because they get reset
            // on each new av_read_frame() call
            PacketPtr = AVCodecInterop.av_packet_alloc();

            // Set up the input frame (not the buffers, since they get reset every time we
            // read a new frame from the input file)
            InputFramePtr = AVUtilInterop.av_frame_alloc();
            AVFrame* inputFrame = (AVFrame*)InputFramePtr.ToPointer();
            inputFrame->format = codecContext.sample_fmt;
            inputFrame->nb_samples = codecContext.frame_size;
            inputFrame->channel_layout = codecContext.channel_layout;
            inputFrame->sample_rate = codecContext.sample_rate;
            if (codecContext.frame_size == 0)
            {
                // This handles WAV files that don't really have frames. FFmpeg sets the packet buffer to
                // this size arbitrarily but doesn't tell us what it is ahead of time, so we have to do the math.
                // This comes from wavdec.c and pcmdec.c in libavformat if you're curious.
                int packetSize = Math.Max(WavSize, codecContext.block_align);
                int bytesPerSample = AVCodecInterop.av_get_exact_bits_per_sample(codecContext.codec_id) / 8;
                int samplesPerPacket = packetSize / (codecContext.channels * bytesPerSample);
                inputFrame->nb_samples = samplesPerPacket;
            }
            if (codecContext.channel_layout == 0)
            {
                // This handles PCM / WAV files, which don't come with any channel layout info.
                ChannelLayout = AVUtilInterop.av_get_default_channel_layout(codecContext.channels);
                inputFrame->channel_layout = ChannelLayout;
            }

            // Set up the output frame, which conforms to Discord requirements (48 kHz, stereo).
            // The format can be anything that Opus handles really, but I believe planar float
            // is the fastest since it'll just wind up converting to it anyway so we may as well
            // have swresample do it here for us.
            OutputFramePtr = AVUtilInterop.av_frame_alloc();
            AVFrame* outputFrame = (AVFrame*)OutputFramePtr.ToPointer();
            outputFrame->channel_layout = AV_CH_LAYOUT.AV_CH_LAYOUT_STEREO;
            outputFrame->sample_rate = 48000;
            outputFrame->format = AVSampleFormat.AV_SAMPLE_FMT_FLTP;

            // Set up the swresample context
            SwrContextPtr = SWResampleInterop.swr_alloc();
            result = SWResampleInterop.swr_config_frame(SwrContextPtr, OutputFramePtr, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling context configuration failed: {result}");
            }
            result = SWResampleInterop.swr_init(SwrContextPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling context initialization failed: {result}");
            }

            // Set up the buffers for the output frame, which are persistent
            long delay = SWResampleInterop.swr_get_delay(SwrContextPtr, 48000);
            int outSamples = (int)delay + (inputFrame->nb_samples * 48000 / inputFrame->sample_rate) + 3;
            outputFrame->nb_samples = outSamples;
            result = AVUtilInterop.av_frame_get_buffer(OutputFramePtr, 0);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Output frame buffer allocation failed: {result}");
            }

            // Set up the output capture buffer for playback. At minimum, it basically has to be able to
            // store two frames at once (because we might read a partial frame during the playback loop,
            // leaving some of that frame left over but not enough to continue playback so we have to read
            // another frame). Techincally I suppose the minimum buffer size is
            // linesize / 4 * 2 - Player.MergeBufferLength, but it's simpler just to make it two frames big.
            int bufferSize = outputFrame->linesize[0] / sizeof(float) * 2;
            Buffer = new DecodedAudioBuffer(bufferSize);

            // Last but not least, create the TrackInfo object in case the user wants to see what's going on with the audio stream.
            double timeBaseInSeconds = Stream.time_base.num / (double)Stream.time_base.den;
            double durationInSeconds = Stream.duration * timeBaseInSeconds;
            TimeSpan duration = TimeSpan.FromSeconds(durationInSeconds);
            Info = new TrackInfo(FilePath, codec.long_name, codecContext.channels, codecContext.bit_rate, codecContext.sample_rate, duration);
        }

        /// <summary>
        /// This reads the next audio frame from the file, decodes it, converts it to Discord
        /// format, and stores it for later reading.
        /// </summary>
        /// <returns>True if processing the next frame was successful, or false if the end of
        /// the file has been reached (and looping is disabled).</returns>
        unsafe internal bool ProcessNextFrame()
        {
            // Get pointers for the in and out frames. I wonder if I should just store these
            // as pointers instead of IntPtrs and converting them every time? It probably doesn't
            // add any appreciable overhead, and then the entire struct would have to be flagged
            // as unsafe so I'll leave these here for now.
            AVFrame* inputFrame = (AVFrame*)InputFramePtr.ToPointer();
            AVFrame* outputFrame = (AVFrame*)OutputFramePtr.ToPointer();

            // Try to get the next available frame from the decoder if one is ready.
            AVERROR result = AVCodecInterop.avcodec_receive_frame(Stream.codec, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS && result != AVERROR.AVERROR_EAGAIN)
            {
                throw new Exception($"Error receiving decoded audio frame: {result}");
            }

            // This happens when the decoder's buffer is dry and it needs a new packet.
            if (result == AVERROR.AVERROR_EAGAIN)
            {
                // Read the next packet from the file
                result = AVFormatInterop.av_read_frame(FormatContextPtr, PacketPtr);
                if (result != AVERROR.AVERROR_SUCCESS)
                {
                    // This happens when we're at the end of the file. 
                    if (result == AVERROR.AVERROR_EOF)
                    {
                        // If this file isn't looping, just return to signal that the file's done.
                        if (!Loop)
                        {
                            return false;
                        }

                        // If this file is looping, start it over from the top.
                        result = AVFormatInterop.avformat_seek_file(FormatContextPtr, Stream.index, long.MinValue, Stream.start_time, long.MaxValue, AVSEEK_FLAG.AVSEEK_FLAG_BACKWARD);
                        if (result != AVERROR.AVERROR_SUCCESS)
                        {
                            throw new Exception($"Error resetting stream to the beginning: {result}");
                        }
                        return ProcessNextFrame();
                    }
                    throw new Exception($"Error reading audio packet: {result}");
                }

                // Send the packet over to the decoder
                result = AVCodecInterop.avcodec_send_packet(Stream.codec, PacketPtr);
                if (result != AVERROR.AVERROR_SUCCESS)
                {
                    throw new Exception($"Error decoding audio packet: {result}");
                }

                // Get the decoded raw data from the decoder.
                result = AVCodecInterop.avcodec_receive_frame(Stream.codec, InputFramePtr);
                if (result != AVERROR.AVERROR_SUCCESS)
                {
                    throw new Exception($"Error receiving decoded audio frame: {result}");
                }
            }

            // WAV files don't have their layout set, so if this is a WAV file then we have to
            // explicitly set it for every single frame since the input frame is always regenerated.
            // Otherwise the SwrContext will freak out because it thinks the input format changed.
            if (inputFrame->channel_layout == 0)
            {
                inputFrame->channel_layout = ChannelLayout;
            }

            // Resample the frame to the Discord requirements (48 kHz, 2 channel stereo)
            result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling audio frame failed: {result}");
            }

            // Copy the new data into the managed buffers
            while (outputFrame->nb_samples > 0)
            {
                Buffer.AddIncomingData((IntPtr)outputFrame->data0, (IntPtr)outputFrame->data1, outputFrame->nb_samples);

                // Keep the cycle going until we've exhausted the swrcontext buffer
                result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, IntPtr.Zero);
                if (result != AVERROR.AVERROR_SUCCESS)
                {
                    throw new Exception($"Resampling audio frame failed: {result}");
                }
            }

            // Clean up the packet and input frames to make sure their buffers are released,
            // since ffmpeg insists on reallocating them each time. Note that the output
            // frame doesn't need to be unref'd, because it's only used by swresample which
            // doesn't allocate a new buffer for the frame each time swr_convert_frame is called.
            AVUtilInterop.av_frame_unref(InputFramePtr);
            AVCodecInterop.av_packet_unref(PacketPtr);

            return true;
        }

        /// <summary>
        /// Begins playback of this track.
        /// </summary>
        public void Play()
        {
            Manager.AddTrackToPlaybackList(this);
        }

        /// <summary>
        /// Stops playback of the track.
        /// </summary>
        public void Stop()
        {
            Manager.RemoveTrackFromPlaybackList(this);
        }

        /// <summary>
        /// Resets the track back to the beginning of the file.
        /// </summary>
        internal void Reset()
        {
            AVERROR result = AVFormatInterop.avformat_seek_file(FormatContextPtr, Stream.index, long.MinValue, Stream.start_time, long.MaxValue, AVSEEK_FLAG.AVSEEK_FLAG_BACKWARD);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error resetting stream to the beginning: {result}");
            }
            AVCodecInterop.avcodec_flush_buffers(Stream.codec);
            Buffer.Reset();
            Stopped?.Invoke(this, null);
        }

        /// <summary>
        /// This writes decoded data from this track into the buffer for audio playback.
        /// </summary>
        /// <param name="PlaybackBuffer">The playback buffer, in interleaved (packed) format</param>
        /// <param name="NumberOfSamplesToWrite">The number of decoded samples to write into the playback buffer</param>
        /// <param name="OverwriteExistingData">True to replace whatever's in the playback buffer with the decoded data
        /// in this buffer, false to append this data to whatever's already inside the playback buffer. This is usually set to true
        /// for the first stream in a playback loop iteration, to overwrite the old stale data from the previous loop. After that it's
        /// set to false.</param>
        internal void WriteDataIntoPlaybackBuffer(float[] PlaybackBuffer, int NumberOfSamplesToWrite, bool OverwriteExistingData)
        {
            Buffer.WriteDataIntoPlaybackBuffer(PlaybackBuffer, NumberOfSamplesToWrite, Volume, OverwriteExistingData);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                SWResampleInterop.swr_free(ref SwrContextPtr);
                AVUtilInterop.av_frame_free(ref InputFramePtr);
                AVUtilInterop.av_frame_free(ref OutputFramePtr);
                AVCodecInterop.av_packet_free(ref PacketPtr);
                AVFormatInterop.avformat_free_context(FormatContextPtr);

                disposedValue = true;
            }
        }

        ~AudioTrack()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
