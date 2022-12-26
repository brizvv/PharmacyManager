using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.DataTypes
{
    /// <summary>
    /// Класс описывающий структуру данных товара
    /// </summary>    
    public class Product : BaseTableInfo
    {        
        public int Id { get; private set; }
        public string Name { get; set; }        

        public override string GetTableName()
        {
            return "Products";
        }
    }
}
