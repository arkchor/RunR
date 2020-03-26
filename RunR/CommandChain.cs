using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archersoft.RunR
{
    internal class CommandChain : ICommand
    {
        private readonly IEnumerable<ICommand> _commandsChain;

        public CommandChain(IEnumerable<ICommand> commandsChain)
        {
            _commandsChain = commandsChain;
        }

        public async Task Execute()
        {
            foreach (var command in _commandsChain)
                await command.Execute();
        }
    }
}