using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PrismBot.Services;

namespace PrismBot
{
    class Program
    {
        private static DiscordSocketClient client { get; set; }

        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            using ServiceProvider services = new ServiceCollection()
                .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig() { GatewayIntents = GatewayIntents.All, AlwaysDownloadUsers = true }))
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlerService>()
                .BuildServiceProvider();

            client = services.GetRequiredService<DiscordSocketClient>();
            client.Log += Logger;

            await client.SetStatusAsync(UserStatus.DoNotDisturb);
            await client.SetGameAsync("p?help");

            await client.LoginAsync(TokenType.Bot, "OTM5NDMwNDYzOTUxNDk5Mjg0.Yf4uzA.6CLaVVYwPpg8PQgV52orz8RpDko");
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlerService>().InitializeAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private static Task Logger(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}