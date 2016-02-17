using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolLibrary.Model.Core
{
    public class HcTableInfo : HcDBBaseInfo
    {
        public static List<HcTableInfo> CreateTables(string schema)
        {
            var tables = new List<HcTableInfo>();
            if (HcMySQLToolContext.IsConnected)
            {
                MySqlDataReader DBReader = null;
                try
                {
                    var strSQL = string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE  TABLE_SCHEMA='{0}' AND TABLE_TYPE = 'BASE TABLE'; ", schema);
                    var DBComad = new MySqlCommand(strSQL, HcMySQLToolContext.Connection);
                    DBReader = DBComad.ExecuteReader();

                    while (DBReader.Read())
                    {
                        tables.Add(new HcTableInfo() { Name = DBReader.GetString(0) });
                    }
                    DBReader.Close();
                }
                finally
                {
                    if (DBReader != null)
                    {
                        DBReader.Close();
                    }
                }
            }

            foreach (var table in tables)
            {
                table.Columns = HcColumnInfo.CreateColumns(schema, table.Name);
            }

            return tables;
        }
    }
}
