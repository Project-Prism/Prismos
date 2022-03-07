using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Prismos.Objects
{
    public class FightInstance
    {
        public SocketGuildUser[] Users { get; set; }
        public SocketSlashCommand Interaction { get; set; }
        public int[] Health { get; set; }
        public int[] Defense { get; set; }
        private int turn = 0;
        private bool won = false;
        private Stopwatch watch;

        public FightInstance()
        {
            Task.Factory.StartNew(WatchThread, TaskCreationOptions.LongRunning);
        }

        public async Task UpdateMessage()
        {
            ComponentBuilder builder = new ComponentBuilder()
               .WithButton("Attack", "attackb", ButtonStyle.Primary)
               .WithButton("Defend", "defendb", ButtonStyle.Primary)
               .WithButton("Forfeit", "forfeitb", ButtonStyle.Danger);

            if (won)
            {
                if (turn == 0) turn = 1;
                else turn = 0;
                await Interaction.ModifyOriginalResponseAsync(m => { m.Content = $"{Users[turn].Username} won the fight!";  m.Embeds = CreateEmbeds();});
                Program.fights.Remove(this);
            }
            else
            {
                await Interaction.ModifyOriginalResponseAsync(m => { m.Content = $"{Users[turn].Username}'s turn";  m.Embeds = CreateEmbeds(); m.Components = builder.Build(); });
            }
        }

        public async Task DoAction(SocketMessageComponent comp)
        {
            bool exists = false;
            foreach (var user in Users)
            {
                if (user.Id == comp.User.Id)
                {
                    exists = true;
                    break;
                }
            }

            if (exists)
            {
                if (Users[turn].Id == comp.User.Id)
                {
                    watch.Restart();
                    switch (comp.Data.CustomId)
                    {
                        case "attackb":
                            {
                                int enemyidx = 0;
                                if (turn == 0) enemyidx = 1;

                                Random rng = new Random();
                                int damage = rng.Next(10, 20);
                                damage = damage - (damage * Defense[enemyidx] / 100);
                                Health[enemyidx] -= damage;

                                if (Health[enemyidx] < 1)
                                {
                                    won = true;
                                }
                                break;
                            }
                        case "defendb":
                            {
                                if (Defense[turn] < 50) Defense[turn] += 10;
                                break;
                            }
                        case "forfeitb":
                            {
                                won = true;
                                break;
                            }
                    }

                    if (turn == 0) turn = 1;
                    else turn = 0;

                    await UpdateMessage();
                }
            }
        }

        private void WatchThread()
        {
            watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                if (watch.ElapsedMilliseconds > 600000)
                {
                    Interaction.ModifyOriginalResponseAsync(m => m.Content = $"Session timed out.").GetAwaiter().GetResult();
                    Program.fights.Remove(this);
                    break;
                }
                Thread.Sleep(5000);
            }
        }

        private Embed[] CreateEmbeds()
        {
            EmbedBuilder embed1 = new EmbedBuilder();
            embed1.Title = Users[0].ToString();
            embed1.Description = $"{new Emoji("❤️")} {GetHealthString(Health[0])} {Health[0]}%\n{new Emoji("🛡")} {GetDefenseString(Defense[0])} {Defense[0]}%";

            EmbedBuilder embed2 = new EmbedBuilder();
            embed2.Title = Users[1].ToString();
            embed2.Description = $"{new Emoji("❤️")} {GetHealthString(Health[1])} {Health[1]}%\n{new Emoji("🛡")} {GetDefenseString(Defense[1])} {Defense[1]}%";

            return new Embed[] { embed1.Build(), embed2.Build() };
        }

        private string GetHealthString(int health)
        {
            string healthstring = health.ToString();
            if (healthstring.Length > 1) healthstring = healthstring.Remove(healthstring.Length - 1, 1);
            else if (int.Parse(healthstring) <= 0) healthstring = "0";
            else healthstring = "1";
            int pnum = int.Parse(healthstring);

            string output = string.Empty;
            if (pnum > 0) output += new string('▓', pnum * 2);

            int remainder = (10 - pnum) * 2;
            if (remainder > 0) output += new string('░', remainder);

            return output;
        }

        private string GetDefenseString(int def)
        {
            string defstring = def.ToString();
            if (defstring.Length > 1) defstring = defstring.Remove(defstring.Length - 1, 1);
            else if (int.Parse(defstring) <= 0) defstring = "0";
            else defstring = "1";
            int pnum = int.Parse(defstring);

            string output = string.Empty;
            if (pnum > 0) output += new string('▓', pnum * 2);

            int remainder = (5 - pnum) * 2;
            if (remainder > 0) output += new string('░', remainder);

            return output;
        }
    }
}
