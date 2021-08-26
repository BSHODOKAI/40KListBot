using System;
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
    public class CustomTextParser 
    {
        private string _armyListText;
        public CustomTextParser(string armyListText)
        {
            _armyListText = armyListText;
        }

        public ArmyList ParseTextIntoArmyList()
        {
            var armyList = new ArmyList();

            return armyList;
        }
    }
}
