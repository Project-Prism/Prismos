using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

namespace PrismBot.Modules
{
    public class Random : ModuleBase<SocketCommandContext>
    {
        [Command("hornylevel")]
        public async Task HornyLevel(string? user = null)
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

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

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync($"{user}'s Horniness Level", false, embed.Build());
        }

        [Command("chadlevel")]
        public async Task ChadLevel(string? user = null)
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

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

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync($"{user}'s Chadness Level", false, embed.Build());
        }

        [Command("hornylist")]
        public async Task HornyList()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

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

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [Command("chadlist")]
        public async Task ChadList()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

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

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
    }
}
