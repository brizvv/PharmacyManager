using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyManager.DataTypes;

namespace PharmacyManager.Database
{
    internal interface IDatabase
    {
        void DeleteDataFromTableById(string tableName, int Id);
        void InsertDataInTable(BaseTableInfo data);
        List<string> GetAllProductsInPharmacyByPharmacyId(int pharmacyId);
        List<string> GetAllDataFromTable(string tableName);
        bool IsIdExistsInTable(string tableName, int id);
        bool ConnectToDb();
    }
}
