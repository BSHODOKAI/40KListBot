using Discord.Commands;
using Discord;
using System.Threading.Tasks;

namespace _40KListBot
{
    public class ListModule: ModuleBase<SocketCommandContext>
    {
        [Command("list")]
        [Summary("Gives info for the 40KListBot")]
        public Task ListAsync() 
        {
            var attachments = Context.Message.Attachments;
            //I dont Want to Handle Multiple attachments Right now
            if (attachments.Count == 1)
            {
                foreach (var attachment in attachments) 
                {
                    if (attachment.Filename.ToLower().Contains("html"))
                    {
                        EmbedBuilder builder = new EmbedBuilder();
                        builder.WithTitle("Some Title For An Thing!");

                        Context.Channel.SendMessageAsync("", false, builder.Build());
                    }
                }

            }
            else if (attachments.Count == 0)
            {
                //Plain Text bullshit
            }

            return Task.CompletedTask;
        }
    }
}