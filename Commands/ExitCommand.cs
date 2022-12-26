using PharmacyManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Commands
{
    internal class ExitCommand : ConsoleCommand
    {
        public ExitCommand(IDatabase database) :base("exit", database)
        {
        }
        public override void Execute(string[] args, ObjectTypesStorage objectsStorage)
        {
            Environment.Exit(0);
        }
    }
}
