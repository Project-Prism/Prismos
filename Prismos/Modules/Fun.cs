using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;
using Prismos.Objects;

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

        [SlashCommand("fight", "Fight someone.")]
        public async Task Fight(string user)
        {
            await Context.Interaction.DeferAsync();

            SocketGuildUser user_;
            if (user[0] == '<' && user[user.Length - 1] == '>')
            {
                await Context.Guild.DownloadUsersAsync();
                user_ = Context.Guild.GetUser(ulong.Parse(user.Trim('<', '>').Remove(0, 2)));
            }
            else
            {
                await Context.Guild.DownloadUsersAsync();
                user_ = Context.Guild.GetUser(ulong.Parse(user));
            }

            FightInstance f = new();
            f.Users = new SocketGuildUser[] { Context.Guild.GetUser(Context.User.Id), user_ };
            f.Interaction = Context.Interaction;
            f.Health = new int[] { 100, 100 };
            f.Defense = new int[] { 0, 0 };

            Program.fights.Add(f);
            await f.UpdateMessage();
        }
    }
}
