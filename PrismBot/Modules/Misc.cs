using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;

namespace PrismBot.Modules
{
    public class Misc : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
    {
        [SlashCommand("help", "Help manual")]
        public async Task Help()
        {
            await Context.Interaction.DeferAsync();

            EmbedBuilder miscembed = new();
            miscembed.WithColor(Color.Blue);
            miscembed.WithTitle("Misc Commands");
            miscembed.AddField("/say", "Make the bot say something.");
            miscembed.AddField("/kitty", "Returns a random image of a cat.");
            miscembed.AddField("/reddit {subreddit}", "Grabs a random Reddit post from a specified subreddit");

            EmbedBuilder funembed = new();
            funembed.WithColor(Color.Green);
            funembed.WithTitle("Fun Commands");
            funembed.AddField("/joke", "Returns random joke.");
            funembed.AddField("/meme", "Returns a random meme.");
            funembed.AddField("/greentext", "Returns a random 4chan greentext from...Reddit. I'm too lazy to make a 4chan web scraper.");
            funembed.AddField("/hornylevel {user}", "Returns horny level of {user}. If {user} is empty, returns horny level of message author.");
            funembed.AddField("/chadlevel {user}", "Returns chad level of {user}. If {user} is empty, returns chad level of message author.");
            funembed.AddField("/hornylist", "Returns the top 10 horniest people in the server.");
            funembed.AddField("/chadlist", "Returns the top 10 chaddest people in the server.");

            EmbedBuilder nsfwembed = new();
            nsfwembed.WithColor(Color.DarkRed);
            nsfwembed.WithTitle("NSFW Commands");
            nsfwembed.AddField("/nsfw", "Returns a random NSFW image. This command can only be used in an NSFW channel.");
            nsfwembed.AddField("/hentai", "Returns a random hentai image. This command can only be used in an NSFW channel.");

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embeds = new Embed[] { miscembed.Build(), funembed.Build(), nsfwembed.Build() });
        }

        [SlashCommand("say", "Make the bot say something!")]
        public async Task Say(string message)
        {
            await Context.Interaction.DeferAsync();

            if (message.Contains("@everyone") || message.Contains("@here"))
            {
                await RespondAsync("You think you're being smart huh? You can't get me to ping everyone.");
                return;
            }

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Content = message);
        }
    }
}