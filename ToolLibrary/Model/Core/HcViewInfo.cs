using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ToolLibrary.Model.Core
{
    public class HcViewInfo : HcDBBaseInfo
    {
        public static List<HcViewInfo> CreateViews(string schema)
        {
            var views = new List<HcViewInfo>();
            if (HcMySQLToolContext.IsConnected)
            {
                MySqlDataReader DBReader = null;
                try
                {
                    var strSQL = string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS WHERE  TABLE_SCHEMA='{0}'; ", schema);
                    var DBComad = new MySqlCommand(strSQL, HcMySQLToolContext.Connection);
                    DBReader = DBComad.ExecuteReader();

                    while (DBReader.Read())
                    {
                        views.Add(new HcViewInfo() { Name = DBReader.GetString(0) });
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

            foreach (var view in views)
            {
                view.Columns = HcColumnInfo.CreateColumns(schema, view.Name);
            }

            return views;
        }
    }
}
