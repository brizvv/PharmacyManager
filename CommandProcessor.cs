using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyManager.DataTypes;
using System.Reflection;
using PharmacyManager.Commands;
using PharmacyManager.Database;

namespace PharmacyManager
{
    internal class CommandProcessor
    {
        ObjectTypesStorage _objectsStorage;
        private List<ConsoleCommand> _commands;
                   
        private IDatabase _database;       
        public CommandProcessor(IDatabase database, ObjectTypesStorage objectsStorage)
        {            
            _database = database;
            _objectsStorage = objectsStorage;
            _commands = new List<ConsoleCommand>();
        }
        public void RegisterCommand(ConsoleCommand command)
        {
            _commands.Add(command);
        }

        public void ProcessCommand(string input)
        {
            if (input == null || input == "")
            {
                Console.WriteLine("Не верная команда");
                return;
            }

            int spacePos = input.IndexOf(' ') == -1 ? input.Length : input.IndexOf(' ');
            string commandStr = input.Substring(0, spacePos);

            var command = _commands.Where(x => string.Equals(x.Name, commandStr, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            
            if (command == null)
            {
                Console.WriteLine("Введена неверная команда: " + commandStr);
                return;
            }

            //if (command.Name != "exit" && (input.IndexOf(";") == -1))
            //if (input.Count() > 1 && (input.IndexOf(";") == -1))
            //{
            //    Console.WriteLine(String.Format("Команда {0} должна содержать параметры разделенные ';'", commandStr));
            //    return;
            //}

            var paramsStr = input.Substring(commandStr.Length, input.Length - commandStr.Length).Split(";").Where(x=>x.Length > 0).Select(x => x.Trim()).ToArray();
            command.Execute(paramsStr, _objectsStorage);            
        }        
    }
}
