using System;

namespace Archersoft.RunR
{
    internal class CommandDefinition
    {
        public CommandDefinition(string uniqueName, string displayName, string description, Func<IServiceProvider, ICommand> commandFactoryMethod)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
            Description = description;
            CommandFactoryMethod = commandFactoryMethod;
        }

        public string UniqueName { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public Func<IServiceProvider, ICommand> CommandFactoryMethod { get; }
    }
}