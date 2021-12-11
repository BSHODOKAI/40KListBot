using Discord.Commands;
using Discord;
using System.Threading.Tasks;

namespace _40KListBot
{
    public class Voldemort: ModuleBase<SocketCommandContext>
    {
        [Command("Marshall")]
        [Summary("Somethign Something, Mentioned Voldemort")]
        public Task MarshallAsync() =>
            Context.User.SendMessageAsync("YOU MUST NOT SAY THE NAME OF HE WHO SHALL NOT BE NAMED!");
        
        [Command("help")]
        [Summary("Basic Help for using the 40KListBot")]
        public Task HelpAsync() =>
            ReplyAsync("Send the Command !list with your list as an attachment, the bot will do the rest!");
    }
}