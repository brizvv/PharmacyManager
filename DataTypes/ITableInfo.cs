using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager.DataTypes
{
    internal interface ITableInfo
    {
        List<string> GetColumnsNames();
        List<string> GetColumnsTypes();
        string GetProperyValueByPropertyName(string propertyName);
        string GetObjectName();
        string GetTableName();
    }
}
