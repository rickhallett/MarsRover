using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MarsRover
{
    public class CommandCenter : ICommandCenter
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandCenter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /**
         * SendCommand provides a global API
         */
        public void SendCommand(string command)
        {
            var commandExecuters = GetCommandExecutersInAssembly();

            var executer = commandExecuters.SingleOrDefault(x => x.MatchCommand(command));
            executer?.ExecuteCommand(command);
        }

        /**
         * GetCommandExecutersInAssembly gathers all classes that base of CommandExecuter, and instantiates them
         * with _serviceProvider
         * _serviceProvider contains singletons of Plateau and RoverSquadManager
         */
        private IEnumerable<CommandExecuter> GetCommandExecutersInAssembly()
        {
            return Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(type => type.IsSubclassOf(typeof(CommandExecuter)) && !type.IsAbstract)
                .Select(x => Activator.CreateInstance(x, _serviceProvider) as CommandExecuter)
                .ToList();
            
            // Classes that implement abstract class CommandExecuter
            // PlataeuCommandExecuter 
            // RoverCommandExecuter 
            // RoverSquadCommandExecuter 
            
        }
    }
}