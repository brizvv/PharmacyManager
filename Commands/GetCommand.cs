using Microsoft.Data.SqlClient;
using PharmacyManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Commands
{
    internal class GetCommand : ConsoleCommand
    {
        public GetCommand(IDatabase database) : base("get", database)
        {
        }
        public override void Execute(string[] args, ObjectTypesStorage objectsStorage)
        {
            var entity = objectsStorage.GetBaseTableInfoByObjectName(args[0]);
            if (entity == null)
            {
                Console.WriteLine("Ошибка некорректный параметр после команды get.");
                return;
            }

            try
            {                
                var data = _database.GetAllDataFromTable(entity.GetTableName());
                data.ForEach(x => Console.WriteLine(x));
            }
            catch (SqlException ex)
            {
                Console.WriteLine(string.Format("Ошибка при чтение данных из таблицы {0}: {1}", entity.GetTableName(), ex.Message));
            }
        }
    }
}
