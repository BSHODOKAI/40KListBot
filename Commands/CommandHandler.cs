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


namespace _40KListBot
{
    public class CommandHandler 
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            //Replace if Uplifted with Dependency Injection
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
        }

        private async Task HandleCommandAsync(SocketMessage iSocketMessage)
        {
            var lSocketMessage = iSocketMessage as SocketUserMessage;
            if (iSocketMessage == null) return; //Who the fuck writes code like this?

            int argPos = 0;
            
            var context = new SocketCommandContext(_client, lSocketMessage);

            if (lSocketMessage?.Content?.ToLower()?.Contains("marshall") ?? false) {
                await _commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: null
                );
            }
            if (!(lSocketMessage.HasCharPrefix('!', ref argPos) ||
                  lSocketMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                  lSocketMessage.Author.IsBot)
                  return; //Sample Code like this gives you cancer.

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null
            );
        }
    }
}
