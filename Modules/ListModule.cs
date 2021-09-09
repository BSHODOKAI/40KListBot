using Discord.Commands;
using Discord;
using System;
using System.Net;
using System.Threading;
using System.Text;
using _40KListBot;
using System.Threading.Tasks;

namespace _40KListBot
{
    public class ListModule: ModuleBase<SocketCommandContext>
    {
        [Command("list")]
        [Summary("Gives List Parsed from 40k Listbot for the 40KListBot")]
        public Task ListAsync() 
        {
            var attachments = Context.Message.Attachments;
            //I dont Want to Handle Multiple attachments Right now
            if (attachments.Count == 1)
            {
                foreach (var attachment in attachments) 
                {
                    var webClient = new WebClient();
                    if (attachment.Filename.ToLower().Contains("html"))
                    {
                        string htmlAsString = "";
                        try
                        {
                            htmlAsString = Encoding.UTF8.GetString(webClient.DownloadData(attachment.Url));
                            var armyList = new HtmlParser(htmlAsString)?.ParseHtmlIntoArmyList();
                            var builders = ArmyListParser.ArmyListIntoDiscordRichText(armyList, Context);
                            foreach (var builder in builders) {
                                Context.Channel.SendMessageAsync("", false, builder.Build());
                                Thread.Sleep(250);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else if (attachment.Filename.ToLower().Contains("txt"))
                    {
                        try
                        {
                            var armyList = new CustomTextParser(Encoding.UTF8.GetString(webClient.DownloadData(attachment.Url)))
                                .ParseTextIntoArmyList();
                            var builders = ArmyListParser.ArmyListIntoDiscordRichText(armyList, Context);
                            foreach (var builder in builders) {
                                Context.Channel.SendMessageAsync("", false, builder.Build());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
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