using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using ToolLibrary.Model.Core;
using Utility.Helper.ExcelHelper;

namespace ToolLibrary.Generator.Input.Service.Excel
{
    public class GeneratorFromDTOExcel : IFImportServiceFile
    {
        public void ReadServiceInputFolder(String path, Dictionary<String, HcSubInfo> dic)
        {
            return;
        }

        public void ReadDTOInputFolder(String path, Dictionary<String, HcSubInfo> dic)
        {
            if (dic == null)
            {
                dic = new Dictionary<string, HcSubInfo>();
            }
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            ReadInputDirectory(TheFolder, dic);
        }

        private void ReadInputDirectory(DirectoryInfo folder, Dictionary<String, HcSubInfo> dic)
        {
            //遍历文件夹
            foreach (DirectoryInfo nextFolder in folder.GetDirectories())
            {
                ReadInputDirectory(nextFolder, dic);
            }

            //遍历文件
            foreach (FileInfo nextFile in folder.GetFiles())
            {
                if (nextFile.Extension.Equals("." + Constants.FileExtension))
                {
                    if (nextFile.Name.Length > Constants.DTOFileFistName.Length && nextFile.Name.Substring(0, Constants.DTOFileFistName.Length).Equals(Constants.DTOFileFistName))
                    {
                        var dtoInfo = convertFromDTOExcel(nextFile);
                        var name = nextFile.Name.Replace(Constants.DTOFileFistName, string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("." + Constants.FileExtension, string.Empty);
                        if (dic.ContainsKey(name))
                        {
                            dic[name].DTOArray = dtoInfo;
                        }
                        else
                        {
                            var subInfo = new HcSubInfo();
                            subInfo.Name = name;
                            subInfo.DTOArray = dtoInfo;
                            dic.Add(subInfo.Name, subInfo);
                        }
                    }
                }
            }
        }

        private List<HcDTOInfo> convertFromDTOExcel(FileInfo nextFile)
        {
            var dtoInfo = new List<HcDTOInfo>();

            ExcelOptions.GetExcelData(nextFile.FullName, s => setDTOInfo(s, dtoInfo));
            return dtoInfo;
        }

        private void setDTOInfo(Worksheet sheet, List<HcDTOInfo> dtoList)
        {
            var name = sheet.Name;
            if (name.Length > 3 && name.Substring(name.Length - 3, 3).Equals("DTO"))
            {
                var dtoInfo = new HcDTOInfo();
                Range range = null;
                range = (Range)sheet.Cells[2, 3];
                dtoInfo.Name = range.Value.ToString();

                range = (Range)sheet.Cells[1, 3];
                dtoInfo.Caption = range.Value.ToString();

                int iRow = 5;
                range = (Range)sheet.Cells[iRow, 3];
                var cellValue = range.Value;
                dtoInfo.FieldArray = new List<HcFieldInfo>();
                while (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                {
                    var field = new HcFieldInfo();
                    field.name = cellValue.ToString();
                    range = (Range)sheet.Cells[iRow, 2];
                    field.caption = range.Value.ToString();
                    range = (Range)sheet.Cells[iRow, 4];
                    field.FieldTypeString = range.Value.ToString();

                    dtoInfo.FieldArray.Add(field);
                    iRow += 1;

                    range = (Range)sheet.Cells[iRow, 3];
                    cellValue = range.Value;
                }
                dtoList.Add(dtoInfo);
            }
        }
    }
}
