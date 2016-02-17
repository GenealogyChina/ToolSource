using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ToolLibrary.Generator.Input.Excel;
using ToolLibrary.Generator.Input.Service.Excel;
using ToolLibrary.Generator.Out.Android;
using ToolLibrary.Generator.Out.iOS;
using ToolLibrary.Generator.Out.Service;
using ToolLibrary.Model;
using ToolLibrary.Model.Core;
using ToolLibrary.Type;
using Utility.Zip;

namespace SourceGenerator
{
    public partial class MainFrom : Form
    {
        public MainFrom()
        {
            InitializeComponent();
        }

        private void MainFrom_Load(object sender, EventArgs e)
        {
            this.chkLstBox.Items.Add("Service DTO");
            this.chkLstBox.Items.Add("Message");
            this.chkLstBox.Items.Add("DB Source");
            this.chkLstBox.Items.Add("Values Code");
            this.txtDataBase.Text = ToolLibrary.Constants.DataBase;
            this.txtDataSource.Text = ToolLibrary.Constants.DataSource;
            this.txtUserID.Text = ToolLibrary.Constants.UserID;
            this.txtPassword.Text = ToolLibrary.Constants.Password;

            this.txtImportPath.Text = ToolLibrary.Constants.ImportPath;
            this.txtExportPath.Text = ToolLibrary.Constants.ExportPath;
        }

        private void chkLstBox_SelectedValueChanged(object sender, EventArgs e)
        {
            setInputAirStatus();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if(chk.Checked == true)
            {
                for (int i = 0; i < chkLstBox.Items.Count; i++)
                {
                    this.chkLstBox.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < chkLstBox.Items.Count; i++)
                {
                    this.chkLstBox.SetItemChecked(i, false);
                }
            }

            this.setInputAirStatus();
        }

        private void setInputAirStatus()
        {
            this.txtImportPath.ReadOnly = true;
            this.btnImportPath.Enabled = false;
            this.txtDataSource.ReadOnly = true;
            this.txtDataBase.ReadOnly = true;
            this.txtUserID.ReadOnly = true;
            this.txtPassword.ReadOnly = true;
            this.btnConnect.Enabled = false;

            for (int i = 0; i < chkLstBox.Items.Count; i++)
            {
                if (this.chkLstBox.GetItemChecked(i))
                {
                    if (this.chkLstBox.Items[i].Equals("DB Source"))
                    {
                        this.txtDataSource.ReadOnly = false;
                        this.txtDataBase.ReadOnly = false;
                        this.txtUserID.ReadOnly = false;
                        this.txtPassword.ReadOnly = false;
                        this.btnConnect.Enabled = true;
                    }
                    else
                    {
                        this.txtImportPath.ReadOnly = false;
                        this.btnImportPath.Enabled = true;
                    }
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (HcMySQLToolContext.IsConnected == true)
            {
                HcMySQLToolContext.CloseMySQLDB();
                btn.Text = "DB Connect";
                this.txtDataSource.ReadOnly = false;
                this.txtDataBase.ReadOnly = false;
                this.txtUserID.ReadOnly = false;
                this.txtPassword.ReadOnly = false;
            }
            else
            {
                if (HcMySQLToolContext.ConnectMySQLDB(this.txtDataBase.Text, this.txtDataSource.Text, this.txtUserID.Text, this.txtPassword.Text))
                {
                    this.txtDataSource.ReadOnly = true;
                    this.txtDataBase.ReadOnly = true;
                    this.txtUserID.ReadOnly = true;
                    this.txtPassword.ReadOnly = true;
                    btn.Text = "DB Disconnect";
                }
            }
        }

        private void btnImportPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "Please select a folder";
            dilog.SelectedPath = this.txtImportPath.Text;
            var result = dilog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                this.txtImportPath.Text = dilog.SelectedPath;
            }
        }

        private void btnExportPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "Please select a folder";
            dilog.SelectedPath = this.txtImportPath.Text;
            var result = dilog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                this.txtExportPath.Text = dilog.SelectedPath;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var importPath = this.txtImportPath.Text.Trim();
            var exportPath = this.txtExportPath.Text.Trim();
            for (int i = 0; i < chkLstBox.Items.Count; i++)
            {
                if (this.chkLstBox.GetItemChecked(i))
                {
                    switch(this.chkLstBox.Items[i].ToString())
                    {
                        case "Service DTO":
                            var genService = new GeneratorFromServiceExcel();
                            var getDTO = new GeneratorFromDTOExcel();
                            var androidGen = new GenertorToAndroid();
                            var iosGen = new GenertorToIOS();
                            var serviceGen = new GenertorToService();
                            var dic = new Dictionary<String, HcSubInfo>();

                            genService.ReadServiceInputFolder(importPath, dic);
                            getDTO.ReadDTOInputFolder(importPath, dic);

                            if (Directory.Exists(exportPath + @"\Android"))
                            {
                                Directory.Delete(exportPath + @"\Android", true);
                            }

                            foreach (var subInfo in dic.Values)
                            {
                                serviceGen.WriteOutPut(exportPath, subInfo);
                                iosGen.WriteOutPut(exportPath, subInfo);
                                androidGen.WriteOutPut(exportPath, subInfo);
                            }

                            break;
                        case "Message":
                            GenertorToMessage.WriteOutPut(
                                exportPath, 
                                GeneratorFromMessageExcel.ReadServiceInputFolder(importPath));
                            break;
                        case "DB Source":
                            if(HcMySQLToolContext.IsConnected != true)
                            {
                                MessageBox.Show("Please DB Connect!!!");
                                return;
                            }
                            var writer = new DBWriter();
                            var tables = HcTableInfo.CreateTables(this.txtDataBase.Text);
                            var views = HcViewInfo.CreateViews(this.txtDataBase.Text);
                            writer.WriteServiceDB(this.txtExportPath.Text, tables, views);
                            break;
                        case "Values Code":
                            GenertorToCode.WriteOutPut(
                                exportPath,
                                GeneratorFromCodeExcel.ReadServiceInputFolder(importPath));
                            break;
                        default:
                            break;
                    }
                }
            }

            var zipManager = new ZipManager(exportPath);
            var newFolder = Directory.CreateDirectory(exportPath);
            zipManager.ZipFolder(newFolder.Parent.FullName + @"\" + newFolder.Name + ".zip");

            MessageBox.Show("Export Over!");
        }
    }
}
