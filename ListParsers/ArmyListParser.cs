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
        public static EmbedBuilder ArmyListIntoDiscordRichText(ArmyList iArmyList, SocketCommandContext iContext)
        {
            EmbedBuilder lEmbedBuilder = new EmbedBuilder();
            lEmbedBuilder.WithTitle(iArmyList.Name);
            lEmbedBuilder.WithAuthor(iContext.User.Username);
            foreach (var detachment in iArmyList.Detachments)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var forceOrg in detachment.ForceOrgs)
                {
                    if (forceOrg.Name.Contains("Configuration")) continue;
                    lEmbedBuilder.AddField(forceOrg.Name, "**************************************");
                    //sb.Append(forceOrg.Name);
                    //sb.Append('\n');
                    foreach(var unit in forceOrg.Units)
                    {
                        if (unit.Name.Contains("[12CP]")) continue;
                        
                        sb.Append(unit.Name.ToUpper());
                        sb.Append('\n');
                        
                        foreach(var unitModel in unit.Models)
                        {
                            sb.Append(unitModel.Name);
                            //sb.Append(" Selections:");
                            foreach (var warGear in unitModel.WarGears)
                            {
                                sb.Append(" - ");
                                sb.Append(warGear.Name);
                                if (warGear != unitModel.WarGears[unitModel.WarGears.Count - 1])
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
                    lEmbedBuilder.AddField("Units", myString);
                    sb.Clear();
                }
                //lEmbedBuilder.AddField(detachment.Name, sb.ToString());
            }
            return lEmbedBuilder;
        }
    }
}