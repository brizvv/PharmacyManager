using PharmacyManager.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManager
{
    internal class ObjectTypesStorage
    {
        List<BaseTableInfo> _types;
        public ObjectTypesStorage()
        {
            _types = new List<BaseTableInfo>();
        }

        public void Register(BaseTableInfo typeInfo)
        {
            _types.Add(typeInfo);
        }

        public BaseTableInfo FindObjectTypeByName(string name)
        {
            return _types.Where(x => string.Equals(x.GetObjectName(), name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public string GetTableNameByTypeName(string typeName)
        {                                
            return _types.Where(x => string.Equals(x.GetObjectName(), typeName, StringComparison.OrdinalIgnoreCase)).Select(x=>x.GetTableName()).FirstOrDefault();
        }

        public BaseTableInfo GetBaseTableInfoByObjectName(string name)
        {
            return _types.Where(x => string.Equals(x.GetObjectName(), name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public List<string> GetTableNames()
        {
            return _types.Select(x=>x.GetTableName()).ToList();
        }
    }
}
