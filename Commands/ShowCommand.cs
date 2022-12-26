using Microsoft.Data.SqlClient;
using PharmacyManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Commands
{
    internal class ShowCommand : ConsoleCommand
    {
        public ShowCommand(IDatabase database) : base("show", database)
        {
        }
        public override void Execute(string[] args, ObjectTypesStorage objectsStorage)
        {
            if (!int.TryParse(args[0], out var id))
            {
                Console.WriteLine("Ошибка при получение информации об товаре в аптеке: id аптеки должен быть целым числом!");
                return;
            }
                
            try 
            {
                var data = _database.GetAllProductsInPharmacyByPharmacyId(id);
                data.ForEach(x => Console.WriteLine(x));
            }
            catch (SqlException ex)
            {
                Console.WriteLine(string.Format("Ошибка при получение информации об товаре в аптеке(id={0}): {1}", id, ex.Message));
            }
        }
    }
}
