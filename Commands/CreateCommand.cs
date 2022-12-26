using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyManager.Database;
using PharmacyManager.DataTypes;

namespace PharmacyManager.Commands
{
    /// <summary>
    /// Класс реализующий команду создания новой строки в таблице
    /// </summary>
    internal class CreateCommand : ConsoleCommand
    {
        public CreateCommand(IDatabase database) : base("create", database)
        {
        }
        public override void Execute(string[] args, ObjectTypesStorage objectsStorage)
        {         
            var entity = objectsStorage.GetBaseTableInfoByObjectName(args[0]);           
            if (entity == null)
            {
                Console.WriteLine("Не верный первый параметр после команды create");
                return;
            }

            var colNames = entity.GetColumnsNames();
            if (args.Length < entity.GetColumnsNames().Count)
            {
                Console.WriteLine(String.Format("Не верное количество параметров после команды create, должно быть {0} параметра, а было: {1}",
                                                entity.GetColumnsNames().Count - 1, args.Length.ToString()));
                return;
            }
            
            var paramsValue = args.Skip(1).ToArray();
            int index = 0;
            foreach (var property in entity.GetType().GetProperties().Where(x => !string.Equals(x.Name, "id", StringComparison.OrdinalIgnoreCase)))
            {
                if (property.PropertyType == typeof(int))
                    property.SetValue(entity, int.Parse(paramsValue[index]));
                else
                    property.SetValue(entity, paramsValue[index]);
                index++;
            }                       
            
            if (IsForeignKeysIdCorrect(entity))
                _database.InsertDataInTable(entity);            
        }

        private bool IsForeignKeysIdCorrect(BaseTableInfo entity)
        {
            bool res = true;

            if (entity.GetTableName() == "Storages")
            {
                int pharmacyId = int.Parse(entity.GetProperyValueByPropertyName("PharmacyId"));
                res = _database.IsIdExistsInTable("Pharmacies", pharmacyId);

                if (!res) Console.WriteLine("Ошибка: введен некорректный id аптеки");
            }
            else if (entity.GetTableName() == "Shipments")
            {
                int storageId = int.Parse(entity.GetProperyValueByPropertyName("StorageId"));
                int productId = int.Parse(entity.GetProperyValueByPropertyName("ProductId"));
                res = _database.IsIdExistsInTable("Storages", storageId);

                if (!res)                
                    Console.WriteLine("Ошибка: введен некорректный id склада");                
                else
                {
                    res = _database.IsIdExistsInTable("Products", productId);
                    if (!res) Console.WriteLine("Ошибка: введен некорректный id товара");
                }                
            }
           
            return res;            
        }
    }
}
