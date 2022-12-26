using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PharmacyManager.DataTypes;

namespace PharmacyManager.Database
{
    /// <summary>
    /// Класс урпавляющий взаимодействием с базой данных
    /// </summary>
    internal class DbManager : IDatabase
    {
        private string _connectionString;
        private string _serverName;
        private string _databaseName;
        public DbManager(string serverName, string databaseName)
        {
            _connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", serverName, databaseName);
            _serverName = serverName;
            _databaseName = databaseName;
        }
        public bool ConnectToDb()
        {
            if (!CreateDataBaseIfNotExists(_databaseName, _serverName))
                return false;
            return CreateAllTablesIfNotExists();
        }
        private void DoCommand(string connectStr, string sqlExpression)
        {
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }
        private bool CreateDataBaseIfNotExists(string dbName, string serverName)
        {
            try 
            {
                string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", serverName, "master");
                if (!IsDatabaseExists(connectionString, dbName))
                {
                    DoCommand(connectionString, "CREATE DATABASE " + dbName);
                    Console.WriteLine("База данных создана");
                }
                else
                    Console.WriteLine("База уже существует");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Ошибка при попытке подключения к базе данных: " + ex.Message);
                return false;
            }            
        }
        private void CreateTableIfNotExists(string tableName, string columnsData)
        {
            if (!IsTableExists(_connectionString, tableName))
            {
                DoCommand(_connectionString, string.Format("CREATE TABLE {0} (Id INT PRIMARY KEY IDENTITY, {1}", tableName, columnsData));
                Console.WriteLine(string.Format("Таблица {0} создана", tableName));
            }
            else
                Console.WriteLine(string.Format("Таблица {0} уже существует", tableName));
        }
        private bool CreateAllTablesIfNotExists()
        {
            try
            {
                CreateTableIfNotExists("Products", "Name NVARCHAR(100) NOT NULL)");
                CreateTableIfNotExists("Pharmacies", "Name NVARCHAR(100) NOT NULL, Address NVARCHAR(100) NOT NULL, Phone NVARCHAR(12))");
                CreateTableIfNotExists("Storages", "Name NVARCHAR(100) NOT NULL, PharmacyId INT REFERENCES Pharmacies (Id) ON DELETE CASCADE)");
                CreateTableIfNotExists("Shipments", "ProductCount INT NOT NULL, StorageId INT REFERENCES Storages (Id) ON DELETE CASCADE," +
                                                    " ProductId INT REFERENCES Products(Id) ON DELETE CASCADE)");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Ошибка при попытке создать таблицы в пустой базе данных: " + ex.Message);
                return false;
            }            
        }
        public void InsertDataInTable(BaseTableInfo data)
        {
            var insertData = GetSqlNamesAndValuesStringFromData(data);
            try
            {
                DoCommand(_connectionString, string.Format("INSERT INTO {0} ({1}) Values({2})", data.GetTableName(), insertData.paramsName, insertData.paramsValues));
                Console.WriteLine(string.Format("Добавлены данные в таблицу {0}", data.GetTableName()));
            }
            catch (SqlException ex)
            {
                Console.WriteLine(string.Format("Ошибка при добавление записи в таблицу {0}: {1}", data.GetTableName(), ex.Message));              
            }
            
        }
        private (string paramsName, string paramsValues) GetSqlNamesAndValuesStringFromData(BaseTableInfo data)
        {
            string propertyNames = "";
            string propertyValues = "";
            foreach (var property in data.GetType().GetProperties().Where(x => x.Name != "Id"))
            {
                propertyNames += propertyNames == "" ? property.Name : ", " + property.Name;
                if (property.PropertyType.Name == "String")
                    propertyValues += propertyValues == "" ? "N'" + property.GetValue(data).ToString() + "'" : ", N'" + property.GetValue(data).ToString() + "'";
                else
                    propertyValues += propertyValues == "" ? property.GetValue(data).ToString() : ", " + property.GetValue(data).ToString();
            }
            propertyNames = propertyNames.Trim();

            return (propertyNames, propertyValues);
        }
        public void DeleteDataFromTableById(string tableName, int Id)
        {
            try
            {
                DoCommand(_connectionString, string.Format("DELETE FROM {0} WHERE Id={1}", tableName, Id));
                Console.WriteLine(string.Format("Удалены данные из таблицы {0}", tableName));
            }
            catch (SqlException ex)
            {
                Console.WriteLine(string.Format("Ошибка при удаление записи из таблицы {0}: {1}", tableName, ex.Message));
            }
        }
        public List<string> GetAllDataFromTable(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var resList = new List<string>();
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM " + tableName, connection);
                SqlDataReader reader = command.ExecuteReader();

                var strBuilder = new StringBuilder();
                for (int i = 0; i < reader.FieldCount; i++)
                    strBuilder.Append(reader.GetName(i) + " ");

                resList.Add(strBuilder.ToString().Trim());

                while (reader.Read())
                {
                    strBuilder.Clear();
                    for (int i = 0; i < reader.FieldCount; i++)
                        strBuilder.Append(reader.GetValue(i).ToString() + " ");
                    resList.Add(strBuilder.ToString().Trim());
                }

                reader.Close();
                return resList;
            }
        }
        private List<string> GetNamesFromTable(string connectStr, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                var resList = new List<string>();
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Name FROM " + tableName, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                    resList.Add(reader.GetValue(0).ToString());

                reader.Close();
                return resList;
            }
        }
        private bool IsDatabaseExists(string connectStr, string databaseName)
        {
            return GetNamesFromTable(connectStr, "sys.databases").Contains(databaseName);
        }
        private bool IsTableExists(string connectStr, string tableName)
        {
            return GetNamesFromTable(connectStr, "sys.objects").Contains(tableName);
        }
        public List<string> GetAllProductsInPharmacyByPharmacyId(int pharmacyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var resList = new List<string>();
                connection.Open();
                string sqlString = string.Format("with PharmacyInfo as" +
                "(SELECT Storages.Id,Shipments.ProductCount,Shipments.ProductId,Products.Name " +
                "FROM Storages " +
                "JOIN Shipments ON Storages.PharmacyId = {0} AND Storages.Id = Shipments.StorageId " +
                "JOIN Products ON Shipments.ProductId = Products.Id)" +
                "select Name, SUM(ProductCount) AS TotalCount " +
                "From PharmacyInfo " +
                "GROUP BY Name", pharmacyId);

                SqlCommand command = new SqlCommand(sqlString, connection);
                SqlDataReader reader = command.ExecuteReader();

                var strBuilder = new StringBuilder();
                for (int i = 0; i < reader.FieldCount; i++)
                    strBuilder.Append(reader.GetName(i) + " ");

                resList.Add(strBuilder.ToString().Trim());

                while (reader.Read())
                {
                    strBuilder.Clear();
                    for (int i = 0; i < reader.FieldCount; i++)
                        strBuilder.Append(reader.GetValue(i).ToString() + " ");
                    resList.Add(strBuilder.ToString().Trim());
                }

                reader.Close();
                return resList;
            }
        }
        public bool IsIdExistsInTable(string tableName, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(string.Format("SELECT * FROM {0} WHERE Id = {1}", tableName, id.ToString()), connection);
                SqlDataReader reader = command.ExecuteReader();

                bool res = reader.Read();
                reader.Close();
                return res;
            }
        }
        public bool IsDataBaseEmpty(ObjectTypesStorage objects)
        {            
            foreach(var tableName in objects.GetTableNames())
            {
                if (HasTableRows(_connectionString, tableName))
                    return true;
            }

            return false;
        }
        private bool HasTableRows(string connectStr, string tableName)
        {
            bool res = false;
            using (SqlConnection connection = new SqlConnection(connectStr))
            {                
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM " + tableName, connection);
                SqlDataReader reader = command.ExecuteReader();

                res = reader.HasRows;                                
                reader.Close();                
            }
            
            return res;
        }
    }
}
