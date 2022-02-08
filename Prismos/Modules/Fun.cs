using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Prismos.Modules
{
    public class Fun : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
    {
        [SlashCommand("joke", "Tells a joke.")]
        public async Task Joke()
        {
            await Context.Interaction.DeferAsync();

            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(new Uri("https://karljoke.herokuapp.com/jokes/random"));
            string content = await response.Content.ReadAsStringAsync();
            var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

            await Context.Interaction.ModifyOriginalResponseAsync(m => m.Content = $"{output["setup"]}\n\n{output["punchline"]}");

            response.Dispose();
            client.Dispose();
        }
    }
}
