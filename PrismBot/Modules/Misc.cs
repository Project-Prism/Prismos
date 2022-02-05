using Discord;
using Discord.Commands;
using Discord.Rest;

namespace HorniBoi.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("HorniBoi Commands");
            embed.AddField("~nread {id}", "Opens book ID from nhentai.");
            embed.AddField("~nsearch {query}", "Returns search results of {query} from nhentai.");
            embed.AddField("~ntagsearch {tags}", "Returns search results of given tags. Tags must be seperated by a space.");

            await smsg.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
    }
}
