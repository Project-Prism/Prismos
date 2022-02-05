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

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("PrismBot Commands");
            embed.AddField("p?say", "Make the bot say something.");
            embed.AddField("p?joke", "Returns random joke.");
            embed.AddField("p?hornylevel {user}", "Returns horny level of {user}. If {user} is empty, returns horny level of message author.");
            embed.AddField("p?chadlevel {user}", "Returns chad level of {user}. If {user} is empty, returns chad level of message author.");
            embed.AddField("p?hornylist", "Returns the top 10 horniest people in the server.");
            embed.AddField("p?chadlist", "Returns the top 10 chaddest people in the server.");
            embed.AddField("p?kitty", "Returns a random image from r/cats");
            embed.AddField("p?meme", "Returns a random meme from Reddit.");
            embed.AddField("p?greentext", "Returns a random 4chan greentext from...Reddit. I'm too lazy to make a 4chan web scraper.");
            embed.AddField("p?nsfw", "Returns a random NSFW image from Reddit. This command can only be used in an NSFW channel.");
            embed.AddField("p?hentai", "Returns a random hentai image from Reddit. This command can only be used in an NSFW channel.");

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
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
