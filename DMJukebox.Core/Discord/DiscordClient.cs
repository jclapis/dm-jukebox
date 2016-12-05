using DMJukebox.Discord.Gateway;
using DMJukebox.Discord.Voice;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DMJukebox.Discord
{
    internal class DiscordClient
    {
        private readonly GatewayConnection Gateway;

        private readonly VoiceClient Voice;

        private readonly CancellationTokenSource CancelSource;

        private AutoResetEvent ConnectWaiter;

        private const string DiscordApiUri = "https://discordapp.com/api";

        public DiscordClient()
        {
            CancelSource = new CancellationTokenSource();
            Gateway = new GatewayConnection(CancelSource.Token);
            Voice = new VoiceClient(CancelSource.Token);
        }

        public async Task Connect()
        {
            Uri gatewayRequestUri = new Uri(new Uri(DiscordApiUri), "gateway");
            HttpClient client = new HttpClient();
            string responseBody = await client.GetStringAsync(gatewayRequestUri);
            GetGatewayResponse response = JsonConvert.DeserializeObject<GetGatewayResponse>(responseBody);

            Uri websocketUri = new Uri($"{response.Url}/?encoding=json&v=5");
            await Gateway.Connect(websocketUri);
            await Voice.Connect(Gateway.BotUserID, Gateway.VoiceSessionID, Gateway.VoiceSessionToken, Gateway.VoiceServerHostname);
            System.Diagnostics.Debug.WriteLine("Connected to Discord!");
        }

    }
}
