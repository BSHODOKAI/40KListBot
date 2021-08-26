using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


namespace _40KListBot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private IConfiguration _config;
        private CommandService _commandService;
        private CommandHandler _commandHandler;
        private readonly IServiceProvider _services;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
            
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _config = Config.BuildConfig();

            _commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });
            _client.Log += Log;
            _commandService.Log += Log;
            //Init Command Handler
            _commandHandler = new _40KListBot.CommandHandler(_client, _commandService);
            //Actually Inject the Command Handling
            await _commandHandler.InstallCommandsAsync();

            var token = _config["token"];

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        //Move This Later
        private Task Log(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString());
            return Task.CompletedTask;
        }
    }
}
