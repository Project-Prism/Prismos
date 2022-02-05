using Discord;
using Discord.Commands;
using Discord.Rest;
using NHentaiSharp.Core;
using NHentaiSharp.Search;

using SearchResult = NHentaiSharp.Search.SearchResult;

namespace HorniBoi.Modules
{
    public class NHentai : ModuleBase<SocketCommandContext>
    {
        [RequireNsfw]
        [Command("nread")]
        public async Task Read(int id)
        {
            Reader b = new Reader(id);
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");
            await b.Send(Context.Channel);
            await smsg.DeleteAsync();
        }

        [Command("nsearch")]
        public async Task Search([Remainder]string search)
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Searching...");
            SearchResult result = await SearchClient.SearchWithTagsAsync(new string[] { search }, 1, PopularitySort.Today);
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Search Results");
            Dictionary<string, GalleryElement> trimmed = new Dictionary<string, GalleryElement>();
            foreach (GalleryElement book in result.elements)
            {
                if (!trimmed.ContainsKey(book.prettyTitle))
                {
                    trimmed.Add(book.prettyTitle, book);
                }
            }
            List<string> links = new List<string>();
            foreach (var t in trimmed) links.Add($"`{t.Value.id}` [{t.Key}](https://nhentai.net/g/{t.Value.id}/)");
            embed.WithDescription(string.Join(Environment.NewLine, links));
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
            await smsg.DeleteAsync();
        }

        [Command("ntagsearch")]
        public async Task TagSearch(params string[] tags)
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Searching...");
            SearchResult result = await SearchClient.SearchWithTagsAsync(tags, 1, PopularitySort.Today);
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Search Results");
            Dictionary<string, GalleryElement> trimmed = new Dictionary<string, GalleryElement>();
            foreach (GalleryElement book in result.elements)
            {
                if (!trimmed.ContainsKey(book.prettyTitle))
                {
                    trimmed.Add(book.prettyTitle, book);
                }
            }
            List<string> links = new List<string>();
            foreach (var t in trimmed) links.Add($"`{t.Value.id}` [{t.Key}](https://nhentai.net/g/{t.Value.id}/)");
            embed.WithDescription(string.Join(Environment.NewLine, links));
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
            await smsg.DeleteAsync();
        }
    }
}