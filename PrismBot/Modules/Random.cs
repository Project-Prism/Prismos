using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;

namespace PrismBot.Modules
{
    public class Random : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
    {
        [SlashCommand("hornylevel", "Get horny level of user.")]
        public async Task HornyLevel(string? user = null)
        {
            await Context.Interaction.DeferAsync();

            if (user == null) user = Context.User.Mention;

            System.Random rng = new();

            int scale = rng.Next(0, 10);
            int remainder = 10 - scale;
            string bar = string.Empty;
            bar += new string('█', scale * 2);
            bar += new string('░', remainder * 2);
            bar += $"  {scale * 10}%";

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithDescription(bar);

            await Context.Interaction.ModifyOriginalResponseAsync(m => { m.Content = $"{user}'s Horniness Level"; m.Embed = embed.Build(); });
        }

        [SlashCommand("chadlevel", "Get chad level of user.")]
        public async Task ChadLevel(string? user = null)
        {
            await Context.Interaction.DeferAsync();

            if (user == null) user = Context.User.Mention;

            System.Random rng = new();
            int scale = rng.Next(0, 10);
            int remainder = 10 - scale;
            string bar = string.Empty;
            bar += new string('█', scale * 2);
            bar += new string('░', remainder * 2);
            bar += $"  {scale * 10}%";

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithDescription(bar);

            await Context.Interaction.ModifyOriginalResponseAsync(m => { m.Content = $"{user}'s Chadness Level"; m.Embed = embed.Build(); });
        }

        [SlashCommand("hornylist", "Get server horny list.")]
        public async Task HornyList()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            await Context.Guild.DownloadUsersAsync();

            int amount = 10;
            if (Context.Guild.Users.Count < 10) amount = Context.Guild.Users.Count;

            List<string> names = new List<string>();
            while (names.Count < amount)
            {
                SocketUser user = Context.Guild.Users.ElementAt(rng.Next(0, Context.Guild.Users.Count - 1));
                if (user.IsBot || user.IsWebhook || names.Contains(user.ToString())) continue;
                names.Add(user.ToString());
            }

            string list = string.Empty;
            int i = 1;
            foreach (string name in names)
            {
                list += $"{i}. {name}\n";
                i++;
            }

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithTitle($"Top horniest people in {Context.Guild.Name}");
            embed.WithFooter("As decided by the Horny Council");
            embed.WithDescription(list);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("chadlist", "Get server chad list.")]
        public async Task ChadList()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            await Context.Guild.DownloadUsersAsync();

            int amount = 10;
            if (Context.Guild.Users.Count < 10) amount = Context.Guild.Users.Count;

            List<string> names = new List<string>();
            while (names.Count < amount)
            {
                SocketUser user = Context.Guild.Users.ElementAt(rng.Next(0, Context.Guild.Users.Count - 1));
                if (user.IsBot || user.IsWebhook || names.Contains(user.ToString())) continue;
                names.Add(user.ToString());
            }

            string list = string.Empty;
            int i = 1;
            foreach (string name in names)
            {
                list += $"{i}. {name}\n";
                i++;
            }

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithTitle($"Top chaddest people in {Context.Guild.Name}");
            embed.WithFooter("As decided by the Chad Council");
            embed.WithDescription(list);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }
    }
}
