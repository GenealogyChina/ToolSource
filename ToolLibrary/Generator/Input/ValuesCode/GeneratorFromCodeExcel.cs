using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using ToolLibrary.Model.Core;
using ToolLibrary.Type;
using Utility.Helper.ExcelHelper;

namespace ToolLibrary.Generator.Input.Excel
{
    public class GeneratorFromCodeExcel
    {
        private static GeneratorFromCodeExcel generator = null;
        public static HcCtgCodeInfo ReadServiceInputFolder(String path)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(path);

            if(generator == null)
            {
                generator = new GeneratorFromCodeExcel();
            }
            return generator.ReadInputDirectory(TheFolder);
        }

        private HcCtgCodeInfo ReadInputDirectory(DirectoryInfo folder)
        {
            HcCtgCodeInfo ctgCodeInfo = null;
            //遍历文件夹
            foreach (DirectoryInfo nextFolder in folder.GetDirectories())
            {
                ctgCodeInfo = ReadInputDirectory(nextFolder);

                if(ctgCodeInfo != null)
                {
                    return ctgCodeInfo;
                }
            }

            //遍历文件
            foreach (FileInfo nextFile in folder.GetFiles())
            {
                if (nextFile.Extension.Equals("." + Constants.FileExtension))
                {
                    var fileName = nextFile.Name.Replace("." + Constants.FileExtension, string.Empty);
                    if (fileName.Length > Constants.CodeFileLastName.Length &&
                        fileName.Substring(fileName.Length - Constants.CodeFileLastName.Length, Constants.CodeFileLastName.Length).Equals(Constants.CodeFileLastName))
                    {
                        ctgCodeInfo = convertFromCodeExcel(nextFile);

                        return ctgCodeInfo;
                    }
                }
            }

            return null;
        }

        private HcCtgCodeInfo convertFromCodeExcel(FileInfo nextFile)
        {
            var ctgCodeInfo = new HcCtgCodeInfo();

            ExcelOptions.GetExcelData(nextFile.FullName, s => setCodeInfo(s, ctgCodeInfo));
            return ctgCodeInfo;
        }

        private void getCtgCodeInfo(Worksheet sheet, HcCtgCodeInfo ctgCodeInfo)
        {
            Range range = null;
            var starRow = 11;
            HcCodeInfo codeInfo = null;
            while (((Range)sheet.Cells[starRow, 5]).Value != null && 
                !string.IsNullOrEmpty(((Range)sheet.Cells[starRow, 5]).Value.ToString()))
            {
                HcCodeItemInfo codeItemInfo = new HcCodeItemInfo();
                range = (Range)sheet.Cells[starRow, 5];
                codeItemInfo.ENName = range.Value.ToString();

                range = (Range)sheet.Cells[starRow, 6];
                codeItemInfo.Code = range.Value.ToString();

                range = (Range)sheet.Cells[starRow, 7];
                codeItemInfo.Caption = range.Value;

                range = (Range)sheet.Cells[starRow, 8];
                codeItemInfo.Note = range.Value;

                range = (Range)sheet.Cells[starRow, 2];
                if (range.Value != null)
                {
                    if (codeInfo != null)
                    {
                        ctgCodeInfo.Codes.Add(codeInfo);
                    }
                    codeInfo = new HcCodeInfo();
                    codeInfo.Name = range.Value;

                    range = (Range)sheet.Cells[starRow, 3];
                    codeInfo.Table = range.Value;

                    range = (Range)sheet.Cells[starRow, 4];
                    codeInfo.Column = range.Value;
                }

                codeInfo.Codes.Add(codeItemInfo);
                starRow += 1;
            }

            if (codeInfo != null)
            {
                ctgCodeInfo.Codes.Add(codeInfo);
            }
        }

        private void getCtgCodeInfoForScene(Worksheet sheet, HcCtgCodeInfo ctgCodeInfo)
        {
            Range range = null;
            var starRow = 11;
            HcCodeInfo codeInfo = null;
            while (((Range)sheet.Cells[starRow, 4]).Value != null &&
                !string.IsNullOrEmpty(((Range)sheet.Cells[starRow, 4]).Value.ToString()))
            {
                HcCodeItemInfo codeItemInfo = new HcCodeItemInfo();
                range = (Range)sheet.Cells[starRow, 4];
                codeItemInfo.ENName = range.Value.ToString();

                range = (Range)sheet.Cells[starRow, 5];
                codeItemInfo.Code = range.Value.ToString();

                range = (Range)sheet.Cells[starRow, 6];
                codeItemInfo.Caption = range.Value;

                range = (Range)sheet.Cells[starRow, 7];
                codeItemInfo.Note = range.Value;

                range = (Range)sheet.Cells[starRow, 2];
                if (range.Value != null)
                {
                    if (codeInfo != null)
                    {
                        ctgCodeInfo.SceneCodes.Add(codeInfo);
                    }
                    codeInfo = new HcCodeInfo();
                    codeInfo.Name = range.Value;

                    range = (Range)sheet.Cells[starRow, 3];
                    codeInfo.ENName = range.Value;
                }

                codeInfo.Codes.Add(codeItemInfo);
                starRow += 1;
            }

            if (codeInfo != null)
            {
                ctgCodeInfo.SceneCodes.Add(codeInfo);
            }
        }

        private void setCodeInfo(Worksheet sheet, HcCtgCodeInfo ctgCodeInfo)
        {
            var name = sheet.Name;
            switch (name)
            {
                case "区分代码一览":
                    // 获得区分代码一览
                    getCtgCodeInfo(sheet, ctgCodeInfo);
                    break;
                case "区分代码一览(画面表示用)":
                    // 区分代码一览(画面表示用)
                    getCtgCodeInfoForScene(sheet, ctgCodeInfo);
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
