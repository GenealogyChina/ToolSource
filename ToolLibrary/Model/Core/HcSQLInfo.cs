using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ToolLibrary.Model.Core
{
    public class HcSQLInfo : HcDBBaseInfo
    {
        public string Key { get; set; }

        public string SQL { get; set; }

        public static HcSQLInfo CreateProduct(string schema, string name, string key, string sql)
        {
            var sqlInfo = new HcSQLInfo();
            if (HcMySQLToolContext.IsConnected)
            {
                sqlInfo.Name = name;
                sqlInfo.Key = key;
                sqlInfo.SQL = sql;
                sqlInfo.Columns = HcColumnInfo.CreateColumns(schema, sqlInfo.Name);
            }

            return sqlInfo;
        }
    }
}
