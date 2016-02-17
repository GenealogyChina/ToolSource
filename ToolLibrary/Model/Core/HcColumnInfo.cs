using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolLibrary.Model.Core
{
    public class HcColumnInfo
    {
        public string ColumnName { get; private set; }

        public bool IsNullAble { get; private set; }

        public MySqlDbType DataType { get; private set; }

        public bool AutoIncrement { get; private set; }

        private string _DataTypeName;
        public string DataTypeName
        {
            get
            {
                return this._DataTypeName;
            }
            private set
            {
                if (value.ToUpper().Equals("INT"))
                {
                    this._DataTypeName = "INTEGER";
                }
                else
                {
                    this._DataTypeName = value.ToUpper();
                }

                DataType = getMySqlDbType(value);
            }
        }

        public object Character_Maximum_Length { get; private set; }

        public bool ColumnKey { get; private set; }

        public static List<HcColumnInfo> CreateColumns(string schema, string tableName)
        {
            var columns = new List<HcColumnInfo>();
            if (HcMySQLToolContext.IsConnected)
            {
                MySqlDataReader DBReader = null;
                try
                {
                    var strSQL = string.Format("SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, COLUMN_KEY, EXTRA FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='{0}' AND TABLE_NAME='{1}'; ", schema, tableName);
                    var DBComad = new MySqlCommand(strSQL, HcMySQLToolContext.Connection);
                    DBReader = DBComad.ExecuteReader();
                    while (DBReader.Read())
                    {
                        columns.Add(
                            new HcColumnInfo()
                            {
                                ColumnName = DBReader.GetString(0),
                                IsNullAble = DBReader.GetString(1).ToUpper().Equals("YES"),
                                DataTypeName = DBReader.GetString(2),
                                Character_Maximum_Length = DBReader.GetValue(3),
                                ColumnKey = DBReader.GetString(4).ToUpper().Equals("PRI"),
                                AutoIncrement = DBReader.GetString(5).ToLower().Equals("auto_increment")
                            });
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
            return columns;
        }

        private static MySqlDbType getMySqlDbType(string dbType)
        {
            MySqlDbType ret = MySqlDbType.VarChar;

            switch (dbType.ToUpper())
            {
                case "CHAR":
                case "VARCHAR":
                case "TEXT":
                    ret = MySqlDbType.VarChar;
                    break;
                case "INT":
                case "TINYINT":
                    ret = MySqlDbType.Int32;
                    break;
                case "BIGINT":
                    ret = MySqlDbType.Int64;
                    break;
                case "DATE":
                    ret = MySqlDbType.Date;
                    break;
                case "BLOB":
                    ret = MySqlDbType.Blob;
                    break;
                case "TIME":
                    ret = MySqlDbType.Time;
                    break;
                case "DECIMAL":
                    ret = MySqlDbType.Decimal;
                    break;
                case "FLOAT":
                    ret = MySqlDbType.Float;
                    break;
                case "DOUBLE":
                    ret = MySqlDbType.Double;
                    break;
                case "DATETIME":
                case "TIMESTAMP":
                    ret = MySqlDbType.Timestamp;
                    break;
                default:
                    ret = (MySqlDbType)Enum.Parse(typeof(MySqlDbType), dbType);
                    break;
            }

            return ret;
        }
    }
}
