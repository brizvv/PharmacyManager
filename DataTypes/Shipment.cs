using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.DataTypes
{
    /// <summary>
    /// Класс описывающий структуру данных партии
    /// </summary>
    internal class Shipment : BaseTableInfo
    {
        public int Id { get; private set; }        
        public int ProductId { get; set; }        
        public int StorageId { get; set; }
        /// <summary>
        /// Количество товара в партии
        /// </summary>
        public int ProductCount { get; set; }

        public override string GetTableName()
        {
            return "Shipments";
        }
    }
}
