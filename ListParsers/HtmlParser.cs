using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public ArmyList ParseHtmlIntoArmyList()
        {
            var armyList = new ArmyList();
            //I hate Xpath, Make it better later.
            var node =  _htmlDoc.DocumentNode.SelectSingleNode("//h1");
            armyList.Name = node?.InnerText ?? "Default List Name";
            //Split all this crap into separate functions for testing/maintenance
            armyList.Detachments = new List<Detachment>();
            var detachmentNodes = _htmlDoc.DocumentNode.SelectNodes("//li[@class='force']");
            foreach (var detachmentNode in detachmentNodes)
            {
                var detachment = new Detachment();
                detachment.Name = GetChildDocumentNodesFromDocumentNode("//h2", detachmentNode).First().InnerText;
                detachment.ForceOrgs = new List<ForceOrg>();
                var forceOrgNodes = GetChildDocumentNodesFromDocumentNode("//li[@class='category']", detachmentNode);
                /*HtmlDocument htmlInNode = new HtmlDocument();
                htmlInNode.LoadHtml(detachmentNode.InnerHtml);
                var forceOrgNodes = htmlInNode.DocumentNode.SelectNodes("//h3"); */
                foreach (var forceOrgNode in forceOrgNodes)
                {
                    var forceOrg = new ForceOrg();
                    forceOrg.Name = GetChildDocumentNodesFromDocumentNode("//h3", forceOrgNode).First().InnerText;
                    forceOrg.Units = new List<Unit>();
                    var unitNodes = GetChildDocumentNodesFromDocumentNode("//li[@class='rootselection']", forceOrgNode);
                    foreach (var unitNode in unitNodes)
                    {
                        var unit = new Unit();
                        unit.Name = GetChildDocumentNodesFromDocumentNode("//h4", unitNode).First().InnerText;
                        //Maybe?
                        var unitModelNodes = GetChildDocumentNodesFromDocumentNode("//li", unitNode);
                        unit.Models = new List<UnitModel>();
                        //Handle these units with Complex Tables for Wargear
                        if (unitModelNodes.Count == 0)
                        {
                            var unitModel = new UnitModel();
                            unitModel.Name = unit.Name;
                            unitModel.WarGears = new List<WarGear>();
                            var warGear = new WarGear();
                            for (int i = 1; i < 5; i++)
                            {
                                var warGearNodes = GetChildDocumentNodesFromDocumentNode($"//p[{i}]", unitNode);
                                if (warGearNodes.Count > 0) 
                                {
                                    if (warGearNodes[0].InnerText.ToLower().Contains("selections"))
                                    {
                                        warGear.Name = Regex.Replace(warGearNodes[0].InnerText, "(<.*>)", "").Trim();
                                        unitModel.WarGears.Add(warGear);                        
                                        unit.Models.Add(unitModel);
                                    }

                                }
                            }
                        }
                        foreach(var unitModelNode in unitModelNodes)
                        {
                            var unitModel = new UnitModel();
                            unitModel.Name = GetChildDocumentNodesFromDocumentNode("//h4", unitModelNode).First().InnerText;
                            unitModel.WarGears = new List<WarGear>();
                            //Ugh
                            var warGearNodes = GetChildDocumentNodesFromDocumentNode("//p", unitModelNode);
                            var warGear = new WarGear();
                            //TODO: Clean Up
                            warGear.Name = GetChildDocumentNodesFromDocumentNode("//span[@class='italic'][2]", unitModelNode).FirstOrDefault()?.InnerText;
                            unitModel.WarGears.Add(warGear);
                            //Try Again, Fuck Parsing HTML
                            if (warGear.Name == null && unitModelNodes.Count == 1)
                            {
                                HtmlDocument htmlInNode = new HtmlDocument();
                                htmlInNode.LoadHtml(unitNode.InnerHtml);
                                var innerNode = htmlInNode.DocumentNode.SelectSingleNode("//p[1]");
                                warGear.Name = Regex.Match(innerNode.InnerText, "(?<=Selections:).*")?.ToString()?.Trim();
                            }
                            unit.Models.Add(unitModel);
                        }
                        forceOrg.Units.Add(unit);
                    }
                    detachment.ForceOrgs.Add(forceOrg);
                }
                armyList.Detachments.Add(detachment);
            }
            //var forceOrgNodes = _htmlDoc.DocumentNode.SelectNodes("//h3");
            return armyList;
        }

        private HtmlNodeCollection GetChildDocumentNodesFromDocumentNode(string xPath, HtmlNode parentNode)
        {
            HtmlDocument htmlInNode = new HtmlDocument();
            htmlInNode.LoadHtml(parentNode.InnerHtml);
            var childNodes = htmlInNode.DocumentNode.SelectNodes(xPath) ?? new HtmlNodeCollection(null);
            return childNodes;
        }
    }
}
