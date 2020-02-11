using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace MarsRover
{
    public class PlateauCommandExecuter : CommandExecuter
    {
        private readonly ILandingSurface _landingSurface;

        public PlateauCommandExecuter(IServiceProvider serviceProvider)
        {
            _landingSurface = serviceProvider.GetService<ILandingSurface>();
        }

        public override Regex RegexCommandPattern => new Regex("^\\d+ \\d+$");

        public override void ExecuteCommand(string command)
        {
            ParseCommand(command, out var width, out var height);
            _landingSurface.Define(width, height);
        }

        private static void ParseCommand(string command, out int width, out int height)
        {
            var splitCommand = command.Split(' ');
            width = int.Parse(splitCommand[0]) + 1;
            height = int.Parse(splitCommand[1]) + 1;
        }
    }
}