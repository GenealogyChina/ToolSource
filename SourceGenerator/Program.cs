using System;
using System.Configuration;
using System.Windows.Forms;

namespace SourceGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ToolLibrary.Constants.Package = config.AppSettings.Settings["Package"].Value;
            ToolLibrary.Constants.ImportPath = config.AppSettings.Settings["ImportPath"].Value;
            ToolLibrary.Constants.ExportPath = config.AppSettings.Settings["ExportPath"].Value;
            ToolLibrary.Constants.FileExtension = config.AppSettings.Settings["FileExtension"].Value;
            ToolLibrary.Constants.ServiceFileFistName = config.AppSettings.Settings["ServiceFileFistName"].Value;
            ToolLibrary.Constants.DTOFileFistName = config.AppSettings.Settings["DTOFileFistName"].Value;
            ToolLibrary.Constants.ServiceFirstName = config.AppSettings.Settings["ServiceFirstName"].Value;
            ToolLibrary.Constants.ProjectName = config.AppSettings.Settings["ProjectName"].Value;
            ToolLibrary.Constants.CompanyName = config.AppSettings.Settings["CompanyName"].Value;

            ToolLibrary.Constants.DataBase = config.AppSettings.Settings["DataBase"].Value;
            ToolLibrary.Constants.DataSource = config.AppSettings.Settings["DataSource"].Value;
            ToolLibrary.Constants.UserID = config.AppSettings.Settings["UserID"].Value;
            ToolLibrary.Constants.Password = config.AppSettings.Settings["Password"].Value;

            ToolLibrary.Constants.MessageFileLastName = config.AppSettings.Settings["MessageFileLastName"].Value;
            ToolLibrary.Constants.CustomerMessageSheet = config.AppSettings.Settings["CustomerMessageSheet"].Value;
            ToolLibrary.Constants.CommonMessageSheet = config.AppSettings.Settings["CommonMessageSheet"].Value;
            ToolLibrary.Constants.ClientMessageSheet = config.AppSettings.Settings["ClientMessageSheet"].Value;
            ToolLibrary.Constants.CodeFileLastName = config.AppSettings.Settings["CodeFileLastName"].Value;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrom());
        }
    }
}
