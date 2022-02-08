using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Prismos.Modules
{
    public class Moderation : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
    {
        [RequireUserPermission(GuildPermission.KickMembers)]
        [SlashCommand("kick", "Kick user.")]
        public async Task Kick(string user, string? reason = null)
        {
            await Context.Interaction.DeferAsync();

            SocketGuildUser user_;
            if (user[0] == '<' && user[user.Length - 1] == '>')
            {
                await Context.Guild.DownloadUsersAsync();
                user_ = Context.Guild.GetUser(ulong.Parse(user.Trim('<', '>').Remove(0, 2)));
            }
            else
            {
                await Context.Guild.DownloadUsersAsync();
                user_ = Context.Guild.GetUser(ulong.Parse(user));
            }

            await user_.KickAsync(reason);
            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Content = $"{user_.Mention} has been kicked!");
        }

        [RequireUserPermission(GuildPermission.BanMembers)]
        [SlashCommand("ban", "Ban user.")]
        public async Task Ban(string user, string? reason = null)
        {
            await Context.Interaction.DeferAsync();

            SocketGuildUser user_;
            if (user[0] == '<' && user[user.Length - 1] == '>')
            {
                await Context.Guild.DownloadUsersAsync();
                user_ = Context.Guild.GetUser(ulong.Parse(user.Trim('<', '>').Remove(0, 2)));
            }
            else
            {
                await Context.Guild.DownloadUsersAsync();
                user_ = Context.Guild.GetUser(ulong.Parse(user));
            }

            await user_.BanAsync(7, reason);
            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Content = $"{user_.Mention} has been banned!");
        }
    }
}
