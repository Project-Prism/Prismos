using Newtonsoft.Json;

namespace Prismos.Services
{
    public struct RedditPost
    {
        public string Title { get; set; }
        public string SelfText { get; set; }
        public string Url { get; set; }
        public string RedditUrl { get; set; }
        public string Author { get; set; }
    }

    public class RedditService
    {
        public async Task<RedditPost[]> GetAny(string subreddit)
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync($"https://reddit.com/r/{subreddit}/top/.json?limit=50");
            string content = await response.Content.ReadAsStringAsync();
            dynamic? json = JsonConvert.DeserializeObject(content);
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
            response.Dispose();
            client.Dispose();
            return results.ToArray();
        }

        public async Task<RedditPost[]> GetImages(string subreddit, bool nsfw = false)
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync($"https://reddit.com/r/{subreddit}/top/.json?limit=50");
            string content = await response.Content.ReadAsStringAsync();
            dynamic? json = JsonConvert.DeserializeObject(content);
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
            response.Dispose();
            client.Dispose();
            return results.ToArray();
        }
    }
}
