using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Prismos.Services;

namespace Prismos
{
    class Program
    {
        private static DiscordSocketClient? client { get; set; }
        private static InteractionService? iservice { get; set; }

        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            using ServiceProvider services = new ServiceCollection()
                .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig() { GatewayIntents = GatewayIntents.GuildMembers | GatewayIntents.GuildPresences | GatewayIntents.AllUnprivileged, AlwaysDownloadUsers = true }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<RedditService>()
                .BuildServiceProvider();

            client = services.GetRequiredService<DiscordSocketClient>();
            client.Log += Logger;

            iservice = services.GetRequiredService<InteractionService>();
            iservice.Log += Logger;

            client.JoinedGuild += JoinedGuild;
            client.Ready += async () =>
            {
                foreach (SocketGuild guild in client.Guilds)
                {
                    await iservice.RegisterCommandsToGuildAsync(guild.Id, true);
                }
            };

            await client.SetStatusAsync(UserStatus.Idle);

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private static Task Logger(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private static async Task JoinedGuild(SocketGuild guild)
        {
            await iservice.RegisterCommandsToGuildAsync(guild.Id, true);
        }
    }
}