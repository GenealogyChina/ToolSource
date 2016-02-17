using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using ToolLibrary.Model.Core;
using ToolLibrary.Type;
using Utility.Helper.ExcelHelper;

namespace ToolLibrary.Generator.Input.Excel
{
    public class GeneratorFromMessageExcel
    {
        private static GeneratorFromMessageExcel generator = null;
        public static HcMessageDivisionInfo ReadServiceInputFolder(String path)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(path);

            if(generator == null)
            {
                generator = new GeneratorFromMessageExcel();
            }
            return generator.ReadInputDirectory(TheFolder);
        }

        private HcMessageDivisionInfo ReadInputDirectory(DirectoryInfo folder)
        {
            HcMessageDivisionInfo msgDivInfo = null;
            //遍历文件夹
            foreach (DirectoryInfo nextFolder in folder.GetDirectories())
            {
                msgDivInfo = ReadInputDirectory(nextFolder);

                if(msgDivInfo != null)
                {
                    return msgDivInfo;
                }
            }

            //遍历文件
            foreach (FileInfo nextFile in folder.GetFiles())
            {
                if (nextFile.Extension.Equals("." + Constants.FileExtension))
                {
                    var fileName = nextFile.Name.Replace("." + Constants.FileExtension, string.Empty);
                    if (fileName.Length > Constants.MessageFileLastName.Length &&
                        fileName.Substring(fileName.Length - Constants.MessageFileLastName.Length, Constants.MessageFileLastName.Length).Equals(Constants.MessageFileLastName))
                    {
                        msgDivInfo = convertFromMessageExcel(nextFile);

                        return msgDivInfo;
                    }
                }
            }

            return null;
        }

        private HcMessageDivisionInfo convertFromMessageExcel(FileInfo nextFile)
        {
            var msgDivInfo = new HcMessageDivisionInfo();

            ExcelOptions.GetExcelData(nextFile.FullName, s => setSubInfo(s, msgDivInfo));
            return msgDivInfo;
        }

        private List<HcMessageInfo> getMessages(Worksheet sheet)
        {
            var lst = new List<HcMessageInfo>();

            Range range = null;
            var starRow = 2;
            while (((Range)sheet.Cells[starRow, 1]).Value != null && 
                !string.IsNullOrEmpty(((Range)sheet.Cells[starRow, 1]).Value.ToString()))
            {
                HcMessageInfo msgInfo = new HcMessageInfo();
                range = (Range)sheet.Cells[starRow, 1];
                msgInfo.ID = range.Value;

                range = (Range)sheet.Cells[starRow, 2];
                msgInfo.Value = range.Value;

                range = (Range)sheet.Cells[starRow, 3];
                msgInfo.Type = Enum.Parse(typeof(EmMessageType), range.Value);

                range = (Range)sheet.Cells[starRow, 4];
                if (range.Value != null)
                {
                    msgInfo.Skip = Enum.ToObject(typeof(EmMessageType), range.Value);
                }

                lst.Add(msgInfo);
                starRow += 1;
            }

            return lst;
        }

        private void setSubInfo(Worksheet sheet, HcMessageDivisionInfo msgDivInfo)
        {
            var name = sheet.Name;
            switch (name)
            {
                case "消息一览":
                    // 获得消息列表
                    msgDivInfo.CustomerMessages = getMessages(sheet);
                    break;
                case "共通消息一览":
                    // 获得消息列表
                    msgDivInfo.CommonMessages = getMessages(sheet);
                    break;
                case "客户端消息一览":
                    // 获得消息列表
                    msgDivInfo.ClientMessages = getMessages(sheet);
                    break;
                default:
                    break;
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
