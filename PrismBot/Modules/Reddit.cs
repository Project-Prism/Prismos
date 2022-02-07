using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Discord.Rest;
using Newtonsoft.Json;

namespace PrismBot.Modules
{
    public struct RedditPost
    {
        public string Title { get; set; }
        public string SelfText { get; set; }
        public string Url { get; set; }
        public string RedditUrl { get; set; }
        public string Author { get; set; }
    }

    public class Reddit : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
    {
        private string[] meme_subreddits = new string[] { "memes", "dankmemes" };
        private string[] nsfw_subreddits = new string[] { "nsfw", "toocuteforporn" };

        [SlashCommand("reddit", "Get a Reddit post.")]
        public async Task Reddit_(string subreddit)
        {
            await Context.Interaction.DeferAsync();

            System.Random rng = new();
            RedditPost[] posts = await Scraper.GetAny(subreddit);
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
            RedditPost[] posts = await Scraper.GetImages("cats");
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
            RedditPost[] posts = await Scraper.GetImages(meme_subreddits[rng.Next(0, meme_subreddits.Length - 1)]);
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
            RedditPost[] posts = await Scraper.GetImages("greentext");
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
            RedditPost[] posts = await Scraper.GetAny("copypasta");
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
            RedditPost[] posts = await Scraper.GetImages(nsfw_subreddits[rng.Next(0, nsfw_subreddits.Length - 1)], true);
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
            RedditPost[] posts = await Scraper.GetImages("hentai", true);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
        }
    }

    public class Scraper
    {
        public static async Task<RedditPost[]> GetAny(string subreddit)
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync($"https://reddit.com/r/{subreddit}/top/.json?limit=50");
            string content = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(content);
            List<RedditPost> results = new();
            foreach (dynamic c in json.data.children)
            {
                RedditPost r = new();
                r.Title = (string)c.data.title;
                r.SelfText = (string)c.data.selftext;
                r.Url = (string)c.data.url;
                r.RedditUrl = "https://www.reddit.com" + (string)c.data.permalink;
                r.Author = (string)c.data.author;
                results.Add(r);
            }
            return results.ToArray();
        }

        public static async Task<RedditPost[]> GetImages(string subreddit, bool nsfw = false)
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync($"https://reddit.com/r/{subreddit}/top/.json?limit=50");
            string content = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(content);
            List<RedditPost> results = new();
            foreach (dynamic c in json.data.children)
            {
                if ((bool)c.data.is_video)
                    continue;

                if ((bool)c.data.over_18 && !nsfw)
                    continue;

                string url = c.data.url;
                if (!url.Contains("jpg") && !url.Contains("png") && !url.Contains("jpeg") && !url.Contains("webp"))
                    continue;

                RedditPost r = new();
                r.Title = (string)c.data.title;
                r.SelfText = (string)c.data.selftext;
                r.Url = url;
                r.RedditUrl = "https://www.reddit.com" + (string)c.data.permalink;
                r.Author = (string)c.data.author;
                results.Add(r);
            }
            return results.ToArray();
        }
    }
}
