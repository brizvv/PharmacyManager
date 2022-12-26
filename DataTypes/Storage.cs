using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PharmacyManager.DataTypes
{
    /// <summary>
    /// Класс описывающий структуру данных склада
    /// </summary>
    internal class Storage : BaseTableInfo
    {
        public int Id { get; private set; }
        public int PharmacyId { get; set; }
        public string Name { get; set; }
        public override string GetTableName()
        {
            return "Storages";
        }
    }
}
