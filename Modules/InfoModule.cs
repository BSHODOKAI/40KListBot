using Discord.Commands;
using System.Threading.Tasks;

namespace _40KListBot
{
    public class InfoModule: ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        [Summary("Gives info for the 40KListBot")]
        public Task InfoAsync() =>
            ReplyAsync("40K List Bot v0.0.1");

    }
}