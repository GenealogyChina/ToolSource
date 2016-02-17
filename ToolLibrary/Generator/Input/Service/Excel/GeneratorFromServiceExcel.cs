using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using ToolLibrary.Generator.Input.Service;
using ToolLibrary.Model.Core;
using ToolLibrary.Type;
using Utility.Helper.ExcelHelper;

namespace ToolLibrary.Generator.Input.Excel
{
    public class GeneratorFromServiceExcel : IFImportServiceFile
    {
        public void ReadServiceInputFolder(String path, Dictionary<String, HcSubInfo> dic)
        {
            if (dic == null)
            {
                dic = new Dictionary<string, HcSubInfo>();
            }
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            ReadInputDirectory(TheFolder, dic);
        }

        public void ReadDTOInputFolder(String path, Dictionary<String, HcSubInfo> dic)
        {
            return;
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
                    if (nextFile.Name.Length > Constants.ServiceFileFistName.Length && nextFile.Name.Substring(0, Constants.ServiceFileFistName.Length).Equals(Constants.ServiceFileFistName))
                    {
                        var subInfo = convertFromServiceExcel(nextFile);
                        if (dic.ContainsKey(subInfo.Name))
                        {
                            dic[subInfo.Name].ServiceArray = subInfo.ServiceArray;
                        }
                        else
                        {
                            dic.Add(subInfo.Name, subInfo);
                        }
                    }
                }
            }
        }

        private HcSubInfo convertFromServiceExcel(FileInfo nextFile)
        {
            var subInfo = new HcSubInfo();

            ExcelOptions.GetExcelData(nextFile.FullName, s => setSubInfo(s, subInfo));
            return subInfo;
        }

        private void setSubInfo(Worksheet sheet, HcSubInfo subInfo)
        {
            var name = sheet.Name;
            if (name.Length > Constants.ServiceFirstName.Length && name.Substring(0, Constants.ServiceFirstName.Length).Equals(Constants.ServiceFirstName))
            {
                if (subInfo.ServiceArray == null)
                {
                    subInfo.ServiceArray = new List<HcServiceInfo>();
                }

                var serviceInfo = new HcServiceInfo();
                Range range = null;

                if (string.IsNullOrEmpty(subInfo.Name))
                {
                    range = (Range)sheet.Cells[3, 9];
                    subInfo.Name = range.Value.ToString();
                }
                serviceInfo.ModelID = subInfo.Name;
                range = (Range)sheet.Cells[4, 9];
                serviceInfo.Caption = range.Value.ToString();
                range = (Range)sheet.Cells[5, 9];
                serviceInfo.Name = range.Value.ToString();
                range = (Range)sheet.Cells[9, 2];
                serviceInfo.Summary = range.Value.ToString();

                range = (Range)sheet.Cells[3, 40];
                string type = range.Value.ToString();
                switch (type)
                {
                    case "Read":
                        serviceInfo.ServiceType = Type.EmServiceType.Read;
                        break;
                    case "Update":
                        serviceInfo.ServiceType = Type.EmServiceType.Update;
                        break;
                    default:
                        break;
                }

                var irow = 20;
                setInDTO(sheet, serviceInfo, ref irow);
                setOutDTO(sheet, serviceInfo, ref irow);
                setInDTOCheck(sheet, serviceInfo, irow);

                subInfo.ServiceArray.Add(serviceInfo);
            }
        }

        private void setInDTOCheck(Worksheet sheet, HcServiceInfo serviceInfo, int starRow)
        {
            Range range = null;
            for (int i = 1; i < 100; i++)
            {
                starRow += 1;
                range = (Range)sheet.Cells[starRow, 2];
                var val = range.Value;
                if (val != null && !string.IsNullOrEmpty(val.ToString()) && val.Equals("输入验证"))
                {
                    break;
                }
            }

            starRow += 2;
            range = (Range)sheet.Cells[starRow, 3];
            var dicCheck = new Dictionary<string, List<EmCheckType>>();
            var cellValue = range.Value;
            while (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
            {
                List<EmCheckType> lstCheck = null;
                if (dicCheck.ContainsKey(cellValue.ToString()))
                {
                    lstCheck = dicCheck[cellValue.ToString()];
                }
                else
                {
                    lstCheck = new List<EmCheckType>();
                }

                range = (Range)sheet.Cells[starRow, 15];
                lstCheck.Add(convertToCheckType(range.Value.ToString()));

                if (dicCheck.ContainsKey(cellValue.ToString()))
                {
                    dicCheck[cellValue.ToString()] = lstCheck;
                }
                else
                {
                    dicCheck.Add(cellValue.ToString(), lstCheck);
                }
                starRow += 1;

                range = (Range)sheet.Cells[starRow, 3];
                cellValue = range.Value;
            }

            foreach (var item in serviceInfo.InDTO.FieldArray)
            {
                if (dicCheck.ContainsKey(item.caption))
                {
                    item.FieldCheckType = dicCheck[item.caption];
                }
            }
        }

        private EmCheckType convertToCheckType(string cellValue)
        {
            EmCheckType checkType = EmCheckType.Normal;
            switch (cellValue)
            {
                case "必须输入":
                    checkType = EmCheckType.MustEnter;
                    break;
            }
            return checkType;
        }

        private void setOutDTO(Worksheet sheet, HcServiceInfo serviceInfo, ref int starRow)
        {
            starRow += 3;
            var inDto = new HcDTOInfo();
            inDto.Caption = serviceInfo.Caption + "的OutDTO";

            var range = (Range)sheet.Cells[starRow, 2];
            inDto.Name = range.Value.ToString();

            starRow += 1;
            range = (Range)sheet.Cells[starRow, 15];
            var cellValue = range.Value;
            inDto.FieldArray = new List<HcFieldInfo>();
            while (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
            {
                var field = new HcFieldInfo();
                field.name = cellValue.ToString();
                range = (Range)sheet.Cells[starRow, 3];
                field.caption = range.Value.ToString();
                range = (Range)sheet.Cells[starRow, 22];
                field.FieldTypeString = range.Value.ToString();

                inDto.FieldArray.Add(field);
                starRow += 1;

                range = (Range)sheet.Cells[starRow, 15];
                cellValue = range.Value;
            }

            serviceInfo.OutDTO = inDto;
        }

        private void setInDTO(Worksheet sheet, HcServiceInfo serviceInfo, ref int irow)
        {
            var inDto = new HcDTOInfo();
            inDto.Caption = serviceInfo.Caption + "的InDTO";

            var range = (Range)sheet.Cells[19, 2];
            inDto.Name = range.Value.ToString();

            range = (Range)sheet.Cells[20, 15];
            var cellValue = range.Value;
            inDto.FieldArray = new List<HcFieldInfo>();
            while (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
            {
                var field = new HcFieldInfo();
                field.name = cellValue.ToString();
                range = (Range)sheet.Cells[irow, 3];
                field.caption = range.Value.ToString();
                range = (Range)sheet.Cells[irow, 22];
                field.FieldTypeString = range.Value.ToString();

                inDto.FieldArray.Add(field);
                irow += 1;

                range = (Range)sheet.Cells[irow, 15];
                cellValue = range.Value;
            }

            serviceInfo.InDTO = inDto;
        }
    }
}
