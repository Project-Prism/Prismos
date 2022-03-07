using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Prismos.Services
{
    public class CommandHandlingService
    {
        private readonly InteractionService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<InteractionService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _discord.SlashCommandExecuted += SlashCommandExecuted;
        }

        private async Task SlashCommandExecuted(SocketSlashCommand command)
        {
            SocketInteractionContext<SocketSlashCommand> ctx = new(_discord, command);
            await _commands.ExecuteCommandAsync(ctx, _services);
        }
    }
}