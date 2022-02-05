using Discord;
using Discord.Commands;
using Discord.Rest;
using Newtonsoft.Json;

namespace PrismBot.Modules
{
    public class Fun : ModuleBase<SocketCommandContext>
    {
        [Command("joke")]
        public async Task Joke()
        {
            RestUserMessage smsg = await Context.Channel.SendMessageAsync("Loading...");

            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(new Uri("https://karljoke.herokuapp.com/jokes/random"));
            string content = await response.Content.ReadAsStringAsync();
            var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

            await smsg.ModifyAsync((MessageProperties m) => { m.Content = $"{output["setup"]}\n\n{output["punchline"]}"; });

            response.Dispose();
            client.Dispose();
        }
    }
}
