using Discord;
using Discord.Commands;
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
    }

    public class Reddit : ModuleBase<SocketCommandContext>
    {
        private string[] meme_subreddits = new string[] { "memes", "dankmemes" };
        private string[] nsfw_subreddits = new string[] { "nsfw", "toocuteforporn" };

        [Command("reddit")]
        public async Task Reddit_(string subreddit)
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            System.Random rng = new();
            RedditPost[] posts = await Scraper.GetImages(subreddit);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithDescription(selected.SelfText);
            embed.WithImageUrl(selected.Url);

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [Command("kitty")]
        public async Task Kitty()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            System.Random rng = new();
            RedditPost[] posts = await Scraper.GetImages("cats");
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithUrl(selected.RedditUrl);
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [Command("meme")]
        public async Task Meme()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            System.Random rng = new();
            RedditPost[] posts = await Scraper.GetImages(meme_subreddits[rng.Next(0, meme_subreddits.Length - 1)]);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithImageUrl(selected.Url);

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [Command("greentext")]
        public async Task Greentext()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            System.Random rng = new();
            RedditPost[] posts = await Scraper.GetImages("greentext");
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithUrl(selected.RedditUrl);
            embed.WithTitle(selected.Title);
            embed.WithImageUrl(selected.Url);

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [RequireNsfw]
        [Command("nsfw")]
        public async Task NSFW()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            System.Random rng = new();
            RedditPost[] posts = await Scraper.GetImages(nsfw_subreddits[rng.Next(0, nsfw_subreddits.Length - 1)], true);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [RequireNsfw]
        [Command("hentai")]
        public async Task Hentai()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            System.Random rng = new();
            RedditPost[] posts = await Scraper.GetImages("hentai", true);
            RedditPost selected = posts[rng.Next(0, posts.Length - 1)];

            EmbedBuilder embed = new();
            embed.WithColor(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            embed.WithImageUrl(selected.Url);

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
    }

    public class Scraper
    {
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
                r.Url = (string)c.data.url;
                r.RedditUrl = "https://www.reddit.com" + (string)c.data.permalink;
                results.Add(r);
            }
            return results.ToArray();
        }
    }
}
