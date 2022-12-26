using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager
{
    /// <summary>
    /// Класс для создания тестовых данных в базе
    /// </summary>
    internal class TestDataCreator
    {
        private List<string> _data;
        CommandProcessor _commandProcessor;
        public TestDataCreator(CommandProcessor commandProcessor)
        {
            _data = new List<string>();
            _commandProcessor = commandProcessor;

            // Products, наименовение
            _data.Add("create product; витамин А;");
            _data.Add("create product; витамин B;");
            _data.Add("create product; витамин C;");
            _data.Add("create product; витамин D;");
            _data.Add("create product; витамин E;");
            _data.Add("create product; витамин F;");

            // Pharmacies, наименовение,адрес, телефон
            _data.Add("create pharmacy; будь здоров; чапаева 1; 1111;");
            _data.Add("create pharmacy; вита; московская 2; 2222;");
            _data.Add("create pharmacy; ригла; пушкина 3; 3333;");

            // Storages, id аптеки, наименование
            _data.Add("create storage; 1; склад будь здоров№1;");
            _data.Add("create storage; 1; склад будь здоров№2;");
            _data.Add("create storage; 1; склад будь здоров№3;");
            _data.Add("create storage; 2; склад вита№1;");
            _data.Add("create storage; 2; склад вита№2;");
            _data.Add("create storage; 3; склад ригла№1;");

            // Shipments, id товара, id склада, количество в партии
            _data.Add("create shipment; 1; 1; 10;");
            _data.Add("create shipment; 1; 2; 20;");
            _data.Add("create shipment; 1; 4; 10;");

            _data.Add("create shipment; 2; 3; 10;");
            _data.Add("create shipment; 2; 5; 20;");

            _data.Add("create shipment; 3; 4; 10;");
            _data.Add("create shipment; 3; 6; 20;");

            _data.Add("create shipment; 4; 3; 10;");
            _data.Add("create shipment; 4; 5; 20;");

            _data.Add("create shipment; 5; 1; 10;");
            _data.Add("create shipment; 5; 2; 20;");

            _data.Add("create shipment; 6; 3; 10;");
            _data.Add("create shipment; 6; 4; 20;");
        }
        public void AddTestDataToDatabase()
        {
            _data.ForEach(x => _commandProcessor.ProcessCommand(x));            
        }
    }
}
