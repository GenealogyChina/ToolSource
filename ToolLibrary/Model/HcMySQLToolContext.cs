using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Utility.DataType;

namespace ToolLibrary.Model
{
    public class HcMySQLToolContext
    {
        private MySqlConnection _Connection = null;

        private bool _IsConnected = false;

        protected static HcMySQLToolContext obj { get; set; }

        public static MySqlConnection Connection
        {
            get
            {
                return obj._Connection;
            }
        }

        public static bool IsConnected
        {
            get
            {
                return obj._IsConnected;
            }
        }

        public static List<HuListData> Schemas 
        {
            get
            {
                return loadSchemas();
            }
        }

        private static List<HuListData> loadSchemas()
        {
            var _Schemas = new List<HuListData>();
            if (obj._IsConnected)
            {
                MySqlDataReader DBReader = null;
                try
                {
                    var DBComad = new MySqlCommand("SELECT TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES GROUP BY TABLE_SCHEMA", HcMySQLToolContext.Connection);
                    DBReader = DBComad.ExecuteReader();
                    _Schemas.Clear();
                    while (DBReader.Read())
                    {
                        _Schemas.Add(new HuListData { Text = DBReader.GetString(0), Data = DBReader.GetString(0) });
                    }
                    DBReader.Close();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
                finally
                {
                    if (DBReader != null)
                    {
                        DBReader.Close();
                    }
                }
            }
            return _Schemas;
        }

        static HcMySQLToolContext()
        {
            obj = new HcMySQLToolContext();
        }

        protected virtual bool connectMySQLDB(string dataBase, string dataSource, string userId, string password)
        {
            try
            {
                var str =
                    string.Format("Database='{0}';Data Source='{1}';User Id='{2}';Password='{3}';",
                    dataBase, dataSource, userId, password);
                this._Connection = new MySqlConnection(str);
                this._Connection.Open();
                this._IsConnected = true;
            }
            catch (Exception ex)
            {
                this._IsConnected = false;
            }
            return this._IsConnected;
        }

        protected virtual void closeMySQLDB()
        {
            if (this._IsConnected == true)
            {
                this._Connection.Close();
                this._IsConnected = false;
            }
        }

        public static bool ConnectMySQLDB(string dataBase, string dataSource, string userId, string password)
        {
            return obj.connectMySQLDB(dataBase, dataSource, userId, password);
        }

        public static void CloseMySQLDB()
        {
            obj.closeMySQLDB();
        }

        public static bool ExecuteNonQuery(string sql)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = obj._Connection;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            return true;
        }
    }
}
