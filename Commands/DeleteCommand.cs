using PharmacyManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Commands
{
    internal class DeleteCommand : ConsoleCommand
    {        
        public DeleteCommand(IDatabase database):base("delete", database)
        {
        }
        public override void Execute(string[] args, ObjectTypesStorage objectsStorage)
        {
            var tableName = objectsStorage.GetTableNameByTypeName(args[0]);
            if (args.Length < 2)
            {
                Console.WriteLine("Не верное количество параметров после команды delete, должно быть 2 параметра, а было: " + args.Length.ToString());
                return;
            }
            
            if (tableName == null)
            {
                Console.WriteLine("Не верный первый параметр после команды delete");
                return;
            }

            if (!int.TryParse(args[1], out int id))
            {
                Console.WriteLine("Не верный второй параметр после команды delete");
                return;
            }
            
            _database.DeleteDataFromTableById(tableName, id);
        }
    }
}
