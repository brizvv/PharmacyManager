using PharmacyManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.Commands
{
    internal class CleanCommand : ConsoleCommand
    {
        public CleanCommand(IDatabase database) : base("clean", database)
        {
        }
        public override void Execute(string[] args, ObjectTypesStorage objectsStorage)
        {
            Console.Clear();
            Console.WriteLine("***********Меню команд************");

            Console.WriteLine("1-Удалить: delete [x]; [number];(вместо [х] - pharmacy(аптека), product(товар), storage(склад), shipment(партия), вместо [number] номер)");
            Console.WriteLine("пример: delete product; 2; - удаляет второй товар из текущей таблицы товаров");
            Console.WriteLine("");

            Console.WriteLine("2-Создать: create [x]; [param1]; [param2]; [param3];...");
            Console.WriteLine("вместо [х] - pharmacy(аптека), product(товар), storage(склад), shipment(партия)");
            Console.WriteLine("[param1] - для товара это наименовение, пример: create product; антибиотик;");
            Console.WriteLine("[param1..3] - для аптек это наименовение,адрес, телефон, пример: create pharmacy; будь здоров; чапаева 4; 89051234567");
            Console.WriteLine("[param1..2] - для склада это id аптеки, наименование, пример: create storage; 2; склад №2;");
            Console.WriteLine("[param1..3] - для партии это id товара, id склада, количество в партии, пример: create shipment; 2; 3; 10;");
            Console.WriteLine("");

            Console.WriteLine("3-Отбразить: show [number] - весь список товаров и его количество в выбранной аптеке([number]) (количество товара во всех складах аптеки)");
            Console.WriteLine("");

            Console.WriteLine("4-Получить: - get [x]; (все данные таблицы), пример: get pharmacy;");
            Console.WriteLine("вместо [х] - pharmacy(аптека), product(товар), storage(склад), shipment(партия)");

            Console.WriteLine("5-clean - очистить экран");
            Console.WriteLine("6-exit - выйти из программы");
        }
    }
}
