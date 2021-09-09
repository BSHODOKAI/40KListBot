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
        
        [Command("help")]
        [Summary("Basic Help for using the 40KListBot")]
        public Task HeklpAsync() =>
            ReplyAsync("Send the Command !list with your list as an attachment, the bot will do the rest!");
    }
}