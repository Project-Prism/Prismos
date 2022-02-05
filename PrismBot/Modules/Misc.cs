using Discord;
using Discord.Commands;
using Discord.Rest;

namespace PrismBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            EmbedBuilder miscembed = new();
            miscembed.WithColor(Color.Blue);
            miscembed.WithTitle("Misc Commands");
            miscembed.AddField("p?say", "Make the bot say something.");
            miscembed.AddField("p?kitty", "Returns a random image from r/cats.");
            miscembed.AddField("p?reddit {subreddit}", "Grabs a random Reddit post from a specified subreddit");

            EmbedBuilder funembed = new();
            funembed.WithColor(Color.Green);
            funembed.WithTitle("Fun Commands");
            funembed.AddField("p?joke", "Returns random joke.");
            funembed.AddField("p?meme", "Returns a random meme from Reddit.");
            funembed.AddField("p?greentext", "Returns a random 4chan greentext from...Reddit. I'm too lazy to make a 4chan web scraper.");
            funembed.AddField("p?hornylevel {user}", "Returns horny level of {user}. If {user} is empty, returns horny level of message author.");
            funembed.AddField("p?chadlevel {user}", "Returns chad level of {user}. If {user} is empty, returns chad level of message author.");
            funembed.AddField("p?hornylist", "Returns the top 10 horniest people in the server.");
            funembed.AddField("p?chadlist", "Returns the top 10 chaddest people in the server.");

            EmbedBuilder modembed = new();
            modembed.WithColor(Color.Gold);
            modembed.WithTitle("Moderation Commands");
            modembed.AddField("p?ban {user} {reason}", "Bans {user}.");
            modembed.AddField("p?kick {user}", "Kicks {user}.");

            EmbedBuilder nsfwembed = new();
            nsfwembed.WithColor(Color.DarkRed);
            nsfwembed.WithTitle("NSFW Commands");
            nsfwembed.AddField("p?nsfw", "Returns a random NSFW image from Reddit. This command can only be used in an NSFW channel.");
            nsfwembed.AddField("p?hentai", "Returns a random hentai image from Reddit. This command can only be used in an NSFW channel.");

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(embeds: new Embed[] { miscembed.Build(), funembed.Build(), modembed.Build(), nsfwembed.Build() });
        }

        [Command("say")]
        public async Task Say([Remainder] string message)
        {
            if (Context.Message.Content.Contains("@everyone") || Context.Message.Content.Contains("@here"))
            {
                await Context.Channel.SendMessageAsync("You think you're being smart huh? You can't get me to ping everyone.");
                return;
            }
            await Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync(message);
        }
    }
}