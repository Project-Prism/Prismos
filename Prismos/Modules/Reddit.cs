using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Prismos.Services;

namespace Prismos.Modules
{ 
    public class Reddit : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
    {
        public RedditService? rService { get; set; }

        private string[] meme_subreddits = new string[] { "memes", "dankmemes" };
        private string[] nsfw_subreddits = new string[] { "nsfw", "toocuteforporn" };

        [SlashCommand("reddit", "Get a Reddit post.")]
        public async Task Reddit_(string subreddit)
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await rService.GetAny(subreddit);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithDescription(selected.SelfText);
            embed.WithImageUrl(selected.Url);
            embed.WithFooter($"Posted by u/{selected.Author}");

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("kitty", "Get a picture of a cat.")]
        public async Task Kitty()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await rService.GetImages("cats");
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithUrl(selected.RedditUrl);
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("cute", "Get a cute image.")]
        public async Task Cute()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await rService.GetImages("cute");
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithUrl(selected.RedditUrl);
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("meme", "Get a cool meme.")]
        public async Task Meme()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await rService.GetImages(meme_subreddits[rng.Next(0, meme_subreddits.Length - 1)]);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("wholesome", "Get a wholesome meme.")]
        public async Task Wholesome()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await rService.GetImages("wholesomememes");
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("greentext", "Get a 4chan greentext.")]
        public async Task Greentext()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await rService.GetImages("greentext");
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("copypasta", "Get a copypasta.")]
        public async Task Copypasta()
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await rService.GetAny("copypasta");
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithDescription(selected.SelfText);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("nsfw", "Get an NSFW image")]
        public async Task NSFW()
        {
            await Context.Interaction.DeferAsync();

            if (Context.Channel is ITextChannel tc && !tc.IsNsfw)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(m => m.Content = "You can only use this command in an NSFW channel!");
                return;
            }

            System.Random rng = new();
            RedditPost[] posts = await rService.GetImages(nsfw_subreddits[rng.Next(0, nsfw_subreddits.Length - 1)], true);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }

        [SlashCommand("hentai", "Get a hentai image.")]
        public async Task Hentai()
        {
            await Context.Interaction.DeferAsync();

            if (Context.Channel is ITextChannel tc && !tc.IsNsfw)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(m => m.Content = "You can only use this command in an NSFW channel!");
                return;
            }

            System.Random rng = new();
            RedditPost[] posts = await rService.GetImages("hentai", true);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }
    }
}
