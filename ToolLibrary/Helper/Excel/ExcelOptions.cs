using System;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace Utility.Helper.ExcelHelper
{
    public class ExcelOptions
    {
        private static Stopwatch wath = new Stopwatch();

        /// <summary>         
        /// 使用COM读取Excel          
        /// </summary>          
        /// <param name="excelFilePath">路径</param>         
        /// <returns>DataTabel</returns>
        public static void GetExcelData(string excelFilePath, Action<Excel.Worksheet> sheetAction)
        {
            Excel.Application app = new Excel.Application();
            Excel.Sheets sheets;
            Excel.Workbook workbook = null;
            object oMissiong = System.Reflection.Missing.Value;

            wath.Start();
            try
            {
                if (app == null)
                {
                    return;
                }
                workbook = app.Workbooks.Open(excelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
                //将数据读入到DataTable中——Start                       
                sheets = workbook.Worksheets;
                //读取第一张表                  
                if ((Excel.Worksheet)sheets.get_Item(1) == null)
                    return;

                foreach (var worksheet in workbook.Worksheets)
                {
                    sheetAction(worksheet as Excel.Worksheet);
                }
                //string cellContent;
                //int iRowCount = worksheet.UsedRange.Rows.Count;
                //int iColCount = worksheet.UsedRange.Columns.Count;
                //Excel.Range range;
                ////负责列头Start                  
                //DataColumn dc;
                //int ColumnID = 1;
                //range = (Excel.Range)worksheet.Cells[1, 1];
                //while (range.Text.ToString().Trim() != "")
                //{
                //    dc = new DataColumn();
                //    dc.DataType = System.Type.GetType("System.String");
                //    dc.ColumnName = range.Text.ToString().Trim();
                //    dt.Columns.Add(dc);
                //    range = (Excel.Range)worksheet.Cells[1, ++ColumnID];
                //}
                ////End                     
                //for (int iRow = 2; iRow <= iRowCount; iRow++)
                //{
                //    DataRow dr = dt.NewRow();
                //    for (int iCol = 1; iCol <= iColCount; iCol++)
                //    {
                //        range = (Excel.Range)worksheet.Cells[iRow, iCol];
                //        cellContent = (range.Value2 == null) ? "" : range.Text.ToString();
                //        //if (iRow == 1)                          
                //        //{                          
                //        //    dt.Columns.Add(cellContent);                          
                //        //}                          
                //        //else                         
                //        //{                              
                //        dr[iCol - 1] = cellContent;
                //        //}                      
                //    }
                //    //if (iRow != 1)                      
                //    dt.Rows.Add(dr);
                //}
                wath.Stop();
                TimeSpan ts = wath.Elapsed;
                ////将数据读入到DataTable中——End                  
                //return dt;
            }
            catch (Exception ex)
            {
                String error = ex.Message;
                return;
            }
            finally
            {
                workbook.Close(false, oMissiong, oMissiong);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        ///// <summary>          
        ///// 使用COM，多线程读取Excel（1 主线程、4 副线程）          
        ///// </summary>          
        ///// <param name="excelFilePath">路径</param>         
        ///// <returns>DataTabel</returns>          
        //public System.Data.DataTable ThreadReadExcel(string excelFilePath)
        //{
        //    Excel.Application app = new Excel.Application();
        //    Excel.Sheets sheets = null;
        //    Excel.Workbook workbook = null;
        //    object oMissiong = System.Reflection.Missing.Value;
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    wath.Start();
        //    try
        //    {
        //        if (app == null)
        //        {
        //            return null;
        //        }
        //        workbook = app.Workbooks.Open(excelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
        //        //将数据读入到DataTable中——Start                    
        //        sheets = workbook.Worksheets;
        //        Excel.Worksheet worksheet = (Excel.Worksheet)sheets.get_Item(1);
        //        //读取第一张表                  
        //        if (worksheet == null)
        //            return null;
        //        string cellContent;
        //        int iRowCount = worksheet.UsedRange.Rows.Count;
        //        int iColCount = worksheet.UsedRange.Columns.Count;
        //        Excel.Range range;
        //        //负责列头Start                  
        //        DataColumn dc;
        //        int ColumnID = 1;
        //        range = (Excel.Range)worksheet.Cells[1, 1];
        //        //while (range.Text.ToString().Trim() != "")                  
        //        while (iColCount >= ColumnID)
        //        {
        //            dc = new DataColumn();
        //            dc.DataType = System.Type.GetType("System.String");
        //            string strNewColumnName = range.Text.ToString().Trim();
        //            if (strNewColumnName.Length == 0) strNewColumnName = "_1";
        //            //判断列名是否重复                     
        //            for (int i = 1; i < ColumnID; i++)
        //            {
        //                if (dt.Columns[i - 1].ColumnName == strNewColumnName)
        //                    strNewColumnName = strNewColumnName + "_1";
        //            }
        //            dc.ColumnName = strNewColumnName;
        //            dt.Columns.Add(dc);
        //            range = (Excel.Range)worksheet.Cells[1, ++ColumnID];
        //        }
        //        //End                    
        //        //数据大于500条，使用多进程进行读取数据                  
        //        if (iRowCount - 1 > 500)
        //        {
        //            //开始多线程读取数据                     
        //            //新建线程                      
        //            int b2 = (iRowCount - 1) / 10;
        //            DataTable dt1 = new DataTable("dt1");
        //            dt1 = dt.Clone();
        //            SheetOptions sheet1thread = new SheetOptions(worksheet, iColCount, 2, b2 + 1, dt1);
        //            Thread othread1 = new Thread(new ThreadStart(sheet1thread.SheetToDataTable));
        //            othread1.Start();
        //            //阻塞 1 毫秒，保证第一个读取 dt1                      
        //            Thread.Sleep(1);
        //            DataTable dt2 = new DataTable("dt2");
        //            dt2 = dt.Clone();
        //            SheetOptions sheet2thread = new SheetOptions(worksheet, iColCount, b2 + 2, b2 * 2 + 1, dt2);
        //            Thread othread2 = new Thread(new ThreadStart(sheet2thread.SheetToDataTable));
        //            othread2.Start();
        //            DataTable dt3 = new DataTable("dt3");
        //            dt3 = dt.Clone();
        //            SheetOptions sheet3thread = new SheetOptions(worksheet, iColCount, b2 * 2 + 2, b2 * 3 + 1, dt3);
        //            Thread othread3 = new Thread(new ThreadStart(sheet3thread.SheetToDataTable));
        //            othread3.Start();
        //            DataTable dt4 = new DataTable("dt4");
        //            dt4 = dt.Clone();
        //            SheetOptions sheet4thread = new SheetOptions(worksheet, iColCount, b2 * 3 + 2, b2 * 4 + 1, dt4);
        //            Thread othread4 = new Thread(new ThreadStart(sheet4thread.SheetToDataTable));
        //            othread4.Start();
        //            //主线程读取剩余数据                      
        //            for (int iRow = b2 * 4 + 2; iRow <= iRowCount; iRow++)
        //            {
        //                DataRow dr = dt.NewRow();
        //                for (int iCol = 1; iCol <= iColCount; iCol++)
        //                {
        //                    range = (Excel.Range)worksheet.Cells[iRow, iCol];
        //                    cellContent = (range.Value2 == null) ? "" : range.Text.ToString();
        //                    dr[iCol - 1] = cellContent;
        //                }
        //                dt.Rows.Add(dr);
        //            }
        //            othread1.Join();
        //            othread2.Join();
        //            othread3.Join();
        //            othread4.Join();
        //            //将多个线程读取出来的数据追加至 dt1 后面              
        //            foreach (DataRow dr in dt.Rows)
        //                dt1.Rows.Add(dr.ItemArray);
        //            dt.Clear();
        //            dt.Dispose();
        //            foreach (DataRow dr in dt2.Rows)
        //                dt1.Rows.Add(dr.ItemArray);
        //            dt2.Clear();
        //            dt2.Dispose();
        //            foreach (DataRow dr in dt3.Rows)
        //                dt1.Rows.Add(dr.ItemArray);
        //            dt3.Clear();
        //            dt3.Dispose();
        //            foreach (DataRow dr in dt4.Rows)
        //                dt1.Rows.Add(dr.ItemArray);
        //            dt4.Clear();
        //            dt4.Dispose();
        //            return dt1;
        //        }
        //        else
        //        {
        //            for (int iRow = 2; iRow <= iRowCount; iRow++)
        //            {
        //                DataRow dr = dt.NewRow();
        //                for (int iCol = 1; iCol <= iColCount; iCol++)
        //                {
        //                    range = (Excel.Range)worksheet.Cells[iRow, iCol];
        //                    cellContent = (range.Value2 == null) ? "" : range.Text.ToString();
        //                    dr[iCol - 1] = cellContent;
        //                }
        //                dt.Rows.Add(dr);
        //            }
        //        }
        //        wath.Stop();
        //        TimeSpan ts = wath.Elapsed;
        //        //将数据读入到DataTable中——End      
        //        return dt;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        workbook.Close(false, oMissiong, oMissiong);
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
        //        workbook = null;
        //        app.Workbooks.Close();
        //        app.Quit();
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        //        app = null;
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();

        //    }
        //}
    }
}
