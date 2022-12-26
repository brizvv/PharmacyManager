using PharmacyManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Commands
{
    internal abstract class ConsoleCommand
    {
        public string Name { get; }
        private protected IDatabase _database;
        public ConsoleCommand(string name, IDatabase database)
        {
            Name = name;
            _database = database;
        }        
        public abstract void Execute(string[] args, ObjectTypesStorage objectsStorage);
    }
}
