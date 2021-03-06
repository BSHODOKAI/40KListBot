using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using HtmlAgilityPack;

namespace _40KListBot
{
    public class ArmyListParser
    {
        public static List<EmbedBuilder> ArmyListIntoDiscordRichText(ArmyList iArmyList, SocketCommandContext iContext)
        {
            List<EmbedBuilder> lEmbedBuilders = new List<EmbedBuilder>();

            foreach (var detachment in iArmyList.Detachments)
            {
                if (lEmbedBuilders.Count == 0) {
                    var embedBuilder = new EmbedBuilder();
                    embedBuilder.WithTitle(iArmyList.Name);
                    embedBuilder.WithAuthor(iContext.User.Username);
                    lEmbedBuilders.Add(embedBuilder);
                } 
                StringBuilder sb = new StringBuilder();
                var lEmbedBuilder = new EmbedBuilder();
                lEmbedBuilder.AddField(detachment.Name, "*************");
                foreach (var forceOrg in detachment.ForceOrgs)
                {
                    //if (forceOrg.Name.Contains("Configuration")) continue;
                    lEmbedBuilder.AddField(forceOrg.Name, "**************************************");
                    //sb.Append(forceOrg.Name);
                    //sb.Append('\n');
                    foreach(var unit in forceOrg.Units)
                    {
                        //if (unit.Name.Contains("[12CP]")) continue;
                ;         
                        sb.Append(unit.Name.ToUpper());
                        sb.Append('\n');
                        
                        foreach(var unitModel in unit.Models)
                        {
                            sb.Append(unitModel.Name);
                            //sb.Append(" Selections:");
                            foreach (var warGear in unitModel.WarGears)
                            {
                                if (warGear != unitModel.WarGears.Last()) 
                                {
                                    sb.Append(" - ");
                                }
                                sb.Append(warGear.Name);
                                if (warGear != unitModel.WarGears.Last())
                                {
                                    sb.Append(", ");    
                                }
                            }
                            sb.Append('\n');
                        }
                        sb.Append('\n');
                    }
                    var myString = sb.ToString();
                    myString = Regex.Replace(myString, "\\[[0-9]*pts\\]", "");
                    myString = HttpUtility.HtmlDecode(myString);
                    if (myString.Length > 1024) {
                        Console.WriteLine($"String Exceeds Max Length by Discord Embedd, String Value is: {myString}");
                        lEmbedBuilder.AddField("Units", myString.Substring(0, 1020));
                        lEmbedBuilder.AddField("Error:", "Previous field was truncated because it exceeded limits.");
                        iContext.User.SendMessageAsync($"Your List Exceeded the max lengths used by Discord, its in logs but you can submit a ticket to https://github.com/BSHODOKAI/40KListBot"); 
                    } else {
                        lEmbedBuilder.AddField("Units", myString);
                    } 
                    sb.Clear();
                    
                }
                lEmbedBuilders.Add(lEmbedBuilder);
                //lEmbedBuilder.AddField(detachment.Name, sb.ToString());
            }
            return lEmbedBuilders;
        }
    }
}