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
    public class HtmlParser
    {
        HtmlDocument _htmlDoc;
        public HtmlParser(string iHtmlDocAsString)
        {
            _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml(iHtmlDocAsString);
        }

        
    }
}
