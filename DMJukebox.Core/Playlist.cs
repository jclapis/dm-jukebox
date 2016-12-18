/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DMJukebox
{
    /// <summary>
    /// Playlists are collections of tracks that share a common theme,
    /// such as a mood. They're a convenient way to organize tracks and
    /// support playing them all in sequence or randomly.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Playlist
    {
        /// <summary>
        /// The core that manages this playlist (and the others)
        /// </summary>
        internal JukeboxCore Manager { get; set; }

        /// <summary>
        /// The internal collection of tracks that belong to this
        /// playlist
        /// </summary>
        [JsonProperty]
        internal List<AudioTrack> _Tracks { get; set; }
        
        /// <summary>
        /// The backing field for the <see cref="Order"/> property
        /// </summary>
        [JsonProperty]
        internal int _Order { get; set; }

        /// <summary>
        /// The playlist's human-readable name.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// Whether or not shuffle mode (randomization) is enabled
        /// for this playlist during playback.
        /// </summary>
        [JsonProperty]
        public bool IsShuffleEnabled { get; set; }

        /// <summary>
        /// Whether or not loop mode is enabled for this playlist.
        /// </summary>
        [JsonProperty]
        public bool IsLoopEnabled { get; set; }

        /// <summary>
        /// The collection of audio tracks that belong to this
        /// playlist.
        /// </summary>
        public IEnumerable<AudioTrack> Tracks
        {
            get
            {
                return new ReadOnlyCollection<AudioTrack>(_Tracks);
            }
        }

        /// <summary>
        /// This playlist's order relative to the other playlists.
        /// Setting this value will automatically update the other
        /// ones accordingly.
        /// </summary>
        public int Order
        {
            get
            {
                return _Order;
            }
            set
            {
                Manager.UpdatePlaylistOrder(this, value);
            }
        }

        /// <summary>
        /// Creates a new Playlist instance.
        /// </summary>
        internal Playlist()
        {

        }

    }
}
