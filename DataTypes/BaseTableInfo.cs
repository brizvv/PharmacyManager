using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.DataTypes
{
    /// <summary>
    /// Абстрактный класс для получение информации о таблице и связаной с ней сущностью
    /// </summary>
    public abstract class BaseTableInfo : ITableInfo
    {
        public List<string> GetColumnsNames()
        {            
            return this.GetType().GetProperties().Select(p => p.Name).ToList();
        }
        public List<string> GetColumnsTypes()
        {
            return this.GetType().GetProperties().Select(p=> p.GetType().ToString()).ToList();
        }        
        public string GetObjectName()
        { 
            return this.GetType().Name; 
        }    
        public string GetTypeByPropertyName(string name)
        {
            return this.GetType().GetProperties().Where(p => p.Name == name).First().PropertyType.ToString();
        }
        public string GetProperyValueByPropertyName(string propertyName)
        {
            return this.GetType().GetProperties().Where(p => p.Name == propertyName).First().GetValue(this).ToString();
        }
        abstract public string GetTableName();
    }
}
