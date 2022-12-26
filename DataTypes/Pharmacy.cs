using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.DataTypes
{
    /// <summary>
    /// Класс описывающий структуру данных аптеки
    /// </summary>
    public class Pharmacy : BaseTableInfo
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Pharmacy()
        { 
        }
        public Pharmacy(int id, string name, string address, string phone)
        {
            Id = id;
            Name = name;
            Address = address;
            Phone = phone;
        }
        public override string GetTableName()
        {
            return "Pharmacies";
        }
    }
}
