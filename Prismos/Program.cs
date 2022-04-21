using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Prismos.Services;
using Prismos.Objects;

namespace Prismos
{
    class Program
    {
        public static List<FightInstance> fights = new List<FightInstance>();
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
            client.ButtonExecuted += ButtonExecuted;

            iservice = services.GetRequiredService<InteractionService>();
            iservice.Log += Logger;

            client.Ready += async () =>
            {
#if DEBUG
                foreach (SocketGuild guild in client.Guilds)
                {
                    await iservice.RegisterCommandsToGuildAsync(guild.Id, true);
                }
#else
                await iservice.RegisterCommandsGloballyAsync();
#endif
            };

            await client.SetStatusAsync(UserStatus.Idle);

#if DEBUG
            await client.LoginAsync(TokenType.Bot, "");
#else
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
#endif
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private static Task Logger(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private static async Task ButtonExecuted(SocketMessageComponent comp)
        {
            await comp.DeferAsync();
            foreach (var f in fights)
            {
                RestInteractionMessage orgresp = await f.Interaction.GetOriginalResponseAsync();
                if (orgresp.Id == comp.Message.Id)
                {
                    await f.DoAction(comp);
                    break;
                }
            }
        }
    }
}
