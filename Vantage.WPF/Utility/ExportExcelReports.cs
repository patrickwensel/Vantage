using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vantage.Common.Models;
using Vantage.WPF.Models;

namespace Vantage.WPF.Utility
{
    public class ExportExcelReports
    {
        private static string DateFormat = "dd-MMM-yyyy";

        public static void TrainingReport(IList<SelectableDriver> drivers, string absoluteFileName)
        {
            IWorkbook workbook = GetWorkbook(absoluteFileName);

            ISheet mainWorksheet = CreateSheet(workbook, "Training Report");

            // Get All The CellStyles
            ICellStyle titleCellStyle = GetTitleStyle(workbook);
            ICellStyle tableHeaderCellStyle = GetTableHeaderStyle(workbook);
            ICellStyle infoCellStyle = GetInfoStyle(workbook);
            ICellStyle centeredInfoCellStyle = GetCenteredInfoStyle(workbook);
            ICellStyle boldInfoCellStyle = GetBoldInfoFont(workbook);            

            // Add Titles and Date in Report.
            IRow titleRow = mainWorksheet.CreateRow(0);
            var reportTitleCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 3);
            mainWorksheet.AddMergedRegion(reportTitleCellRangeAddress);
            AddCell(titleRow, 0, "Training Report", titleCellStyle);
            AddCell(titleRow, 5, DateTime.Now.ToString(DateFormat), boldInfoCellStyle);

            // Add Headers.
            IRow headerRow = mainWorksheet.CreateRow(2);
            AddCell(headerRow, 0, "Last Name", tableHeaderCellStyle);
            AddCell(headerRow, 1, "First Name", tableHeaderCellStyle);
            AddCell(headerRow, 2, "Group", tableHeaderCellStyle);
            AddCell(headerRow, 3, "Lesson Completed", tableHeaderCellStyle);
            AddCell(headerRow, 4, "Total Lessons", tableHeaderCellStyle);
            AddCell(headerRow, 5, "Course Completed?", tableHeaderCellStyle);

            // Add data now.
            int rowIndex = 3;

            // Adding data now.
            foreach (Driver driver in drivers)
            {
                int completedLessonCount = driver.GroupedAttemptsByLessons.Count(x => x.IsComplete);
                int totalLessonCount = driver.GroupedAttemptsByLessons.Count();
                string couserCompleted = completedLessonCount == totalLessonCount ? "Yes" : "No";
                IRow row = mainWorksheet.CreateRow(rowIndex);
                AddCell(row, 0, driver.LastName, infoCellStyle);
                AddCell(row, 1, driver.FirstName, infoCellStyle);
                AddCell(row, 2, driver.Group.Name, infoCellStyle);
                AddCell(row, 3, completedLessonCount.ToString(), centeredInfoCellStyle);
                AddCell(row, 4, totalLessonCount.ToString(), centeredInfoCellStyle);
                AddCell(row, 5, couserCompleted, centeredInfoCellStyle);
                rowIndex++;
            }

            AutoSizeColumns(mainWorksheet, 6);

            // Creating file now
            using (FileStream fs = new FileStream(absoluteFileName, FileMode.Create))
            {
                workbook.Write(fs);
            }

            workbook.Close();            
        }

        public static void IndividualDriversReport(IList<SelectableDriver> drivers, string absoluteFileName)
        {
            IWorkbook workbook = GetWorkbook(absoluteFileName);

            foreach(Driver driver in drivers)
            {
                ISheet mainWorksheet = CreateSheet(workbook, $"{driver.LastName}{driver.FirstName}_{driver.DriverID}");

                // Get All The CellStyles
                ICellStyle titleCellStyle = GetTitleStyle(workbook);
                ICellStyle tableHeaderCellStyle = GetTableHeaderStyle(workbook);
                ICellStyle infoCellStyle = GetInfoStyle(workbook);
                ICellStyle rightAlignedCellStyle = GetRightAlignedInfoStyle(workbook);
                ICellStyle centeredCellStyle = GetCenteredInfoStyle(workbook);
                ICellStyle boldInfoCellStyle = GetBoldInfoFont(workbook);                

                // Add Titles and Date in Report.
                IRow titleRow = mainWorksheet.CreateRow(0);
                var reportTitleCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 3);
                mainWorksheet.AddMergedRegion(reportTitleCellRangeAddress);
                AddCell(titleRow, 0, "Individual Driver Report", titleCellStyle);
                AddCell(titleRow, 5, DateTime.Now.ToString(DateFormat), boldInfoCellStyle);

                // Add Driver name.
                var cra = new NPOI.SS.Util.CellRangeAddress(2, 2, 2, 3);
                mainWorksheet.AddMergedRegion(cra);
                IRow driverNameRow = mainWorksheet.CreateRow(2);
                AddCell(driverNameRow, 2, $"{driver.LastName}, {driver.FirstName}", tableHeaderCellStyle);

                // Add Headers.
                IRow headerRow = mainWorksheet.CreateRow(4);
                AddCell(headerRow, 0, "Lesson", tableHeaderCellStyle);
                AddCell(headerRow, 1, "High Score", tableHeaderCellStyle);
                AddCell(headerRow, 2, "# of Attempts", tableHeaderCellStyle);
                AddCell(headerRow, 3, "Total Time (Mins)", tableHeaderCellStyle);
                AddCell(headerRow, 4, "Date Completed", tableHeaderCellStyle);

                // Add data now.
                int rowIndex = 5;

                // Adding data now.
                foreach (GroupedAttemptsByLesson attempt in driver.GroupedAttemptsByLessons)
                {
                    int completedLessonCount = driver.GroupedAttemptsByLessons.Count(x => x.IsComplete);
                    int totalLessonCount = driver.GroupedAttemptsByLessons.Count();
                    string couserCompleted = completedLessonCount == totalLessonCount ? "Yes" : "No";
                    IRow row = mainWorksheet.CreateRow(rowIndex);
                    AddCell(row, 0, attempt.Lesson.Name, infoCellStyle);
                    AddCell(row, 1, attempt.HighScore.ToString(), rightAlignedCellStyle);
                    AddCell(row, 2, attempt.TotalAttempts.ToString(), rightAlignedCellStyle);
                    AddCell(row, 3, attempt.TotalTimes.ToString(), rightAlignedCellStyle);
                    AddCell(row, 4, attempt.DateCompleted != null ? attempt.DateCompleted?.ToString("dd-MMM-yyyy") : string.Empty, rightAlignedCellStyle);
                    rowIndex++;
                }

                AutoSizeColumns(mainWorksheet, 6);
            }
           
            // Creating file now
            using (FileStream fs = new FileStream(absoluteFileName, FileMode.Create))
            {
                workbook.Write(fs);
            }

            workbook.Close();
        }

        public static void DetailedLessonReport(IList<SelectableDriver> drivers, string absoluteFileName)
        {
            IWorkbook workbook = GetWorkbook(absoluteFileName);

            foreach (Driver driver in drivers)
            {
                ISheet mainWorksheet = CreateSheet(workbook, $"{driver.LastName}{driver.FirstName}_{driver.DriverID}");

                // Get All The CellStyles
                ICellStyle titleCellStyle = GetTitleStyle(workbook);
                ICellStyle tableHeaderCellStyle = GetTableHeaderStyle(workbook);
                ICellStyle infoCellStyle = GetInfoStyle(workbook);
                ICellStyle rightAlignedCellStyle = GetRightAlignedInfoStyle(workbook);
                ICellStyle centeredCellStyle = GetCenteredInfoStyle(workbook);
                ICellStyle boldInfoCellStyle = GetBoldInfoFont(workbook);
                ICellStyle rightAlignedBoldInfoCellStyle = GetRightAlignedBoldInfoFont(workbook);

                // Add Titles and Date in Report.
                IRow titleRow = mainWorksheet.CreateRow(0);
                var reportTitleCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 3);
                mainWorksheet.AddMergedRegion(reportTitleCellRangeAddress);
                AddCell(titleRow, 0, "Detailed Lesson Report", titleCellStyle);
                AddCell(titleRow, 5, DateTime.Now.ToString(DateFormat), boldInfoCellStyle);

                // Add Driver name.
                var driverNameCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(2, 2, 2, 3);
                mainWorksheet.AddMergedRegion(driverNameCellRangeAddress);
                IRow driverNameRow = mainWorksheet.CreateRow(2);
                AddCell(driverNameRow, 2, $"{driver.LastName}, {driver.FirstName}", tableHeaderCellStyle);

                // Add Headers.
                IRow headerRow = mainWorksheet.CreateRow(4);
                AddCell(headerRow, 0, "Lesson", tableHeaderCellStyle);
                AddCell(headerRow, 1, "High Score", tableHeaderCellStyle);
                AddCell(headerRow, 2, "# of Attempts", tableHeaderCellStyle);
                AddCell(headerRow, 3, "Total Time (Mins)", tableHeaderCellStyle);
                AddCell(headerRow, 4, "Date Completed", tableHeaderCellStyle);

                // Add data now.
                int rowIndex = 5;

                // Adding data now.
                foreach (GroupedAttemptsByLesson attempt in driver.GroupedAttemptsByLessons)
                {
                    int completedLessonCount = driver.GroupedAttemptsByLessons.Count(x => x.IsComplete);
                    int totalLessonCount = driver.GroupedAttemptsByLessons.Count();
                    string couserCompleted = completedLessonCount == totalLessonCount ? "Yes" : "No";
                    IRow row = mainWorksheet.CreateRow(rowIndex);
                    AddCell(row, 0, attempt.Lesson.Name, infoCellStyle);
                    AddCell(row, 1, attempt.HighScore.ToString(), rightAlignedCellStyle);
                    AddCell(row, 2, attempt.TotalAttempts.ToString(), rightAlignedCellStyle);
                    AddCell(row, 3, attempt.TotalTimes.ToString(), rightAlignedCellStyle);
                    AddCell(row, 4, attempt.DateCompleted != null ? attempt.DateCompleted?.ToString("dd-MMM-yyyy") : string.Empty, rightAlignedCellStyle);
                    rowIndex++;

                    if (attempt.GroupedInfractions == null || attempt.GroupedInfractions.Count == 0)
                        continue;

                    // Merge One row before infractions Table.
                    IRow emptyRow1 = mainWorksheet.CreateRow(rowIndex);
                    var emptyMergedCellRow1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 4);
                    mainWorksheet.AddMergedRegion(emptyMergedCellRow1);
                    AddCell(emptyRow1, 0, string.Empty, infoCellStyle);

                    rowIndex++;

                    // Adding Infractions Tables Headers.
                    IRow infractionsHeaderRow = mainWorksheet.CreateRow(rowIndex);
                    AddCell(infractionsHeaderRow, 1, "Infractions", boldInfoCellStyle);
                    AddCell(infractionsHeaderRow, 2, "# of Occurrences", boldInfoCellStyle);
                    AddCell(infractionsHeaderRow, 3, "Points Deducted", boldInfoCellStyle);

                    rowIndex++;

                    foreach (GroupedInfractions groupedInfractions in attempt.GroupedInfractions)
                    {
                        IRow infractionsDataRow = mainWorksheet.CreateRow(rowIndex);
                        AddCell(infractionsDataRow, 1, groupedInfractions.Infraction.Name, infoCellStyle);
                        AddCell(infractionsDataRow, 2, groupedInfractions.Occurances.ToString(), rightAlignedCellStyle);
                        AddCell(infractionsDataRow, 3, groupedInfractions.Deduction.ToString(), rightAlignedCellStyle);

                        rowIndex++;                        
                    }

                    // Adding total of infractions
                    IRow infractionsTotalRow = mainWorksheet.CreateRow(rowIndex);
                    AddCell(infractionsTotalRow, 2, "Total", boldInfoCellStyle);                    
                    AddCell(infractionsTotalRow, 3, attempt.TotalDeduction.ToString(), rightAlignedBoldInfoCellStyle);

                    // Merge First and Last column
                    int infractionTableStartRowIndex = rowIndex - (attempt.GroupedInfractions.Count + 1);
                    var emptyMergedColumn1 = new NPOI.SS.Util.CellRangeAddress(infractionTableStartRowIndex, rowIndex, 0, 0);
                    var emptyMergedColumn2 = new NPOI.SS.Util.CellRangeAddress(infractionTableStartRowIndex, rowIndex, 4, 4);
                    mainWorksheet.AddMergedRegion(emptyMergedColumn1);
                    mainWorksheet.AddMergedRegion(emptyMergedColumn2);

                    rowIndex++;

                    // Merge One row after infractions Table.
                    IRow emptyRow2 = mainWorksheet.CreateRow(rowIndex);
                    var emptyMergedCellRow2 = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 4);
                    mainWorksheet.AddMergedRegion(emptyMergedCellRow2);
                    AddCell(emptyRow2, 0, string.Empty, infoCellStyle);

                    rowIndex++;
                }

                AutoSizeColumns(mainWorksheet, 6);
            }

            // Creating file now
            using (FileStream fs = new FileStream(absoluteFileName, FileMode.Create))
            {
                workbook.Write(fs);
            }

            workbook.Close();
        }

        private static void AddCell(IRow row, int columnIndex, string content, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(columnIndex);
            cell.SetCellValue(content);
            cell.CellStyle = cellStyle;
        }

        private static IWorkbook GetWorkbook(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName.Contains(".xlsx"))
                return new XSSFWorkbook();
            else
                return new HSSFWorkbook();
        }        

        private static ISheet CreateSheet(IWorkbook workbook, string name)
        {
            return workbook.CreateSheet(name);
        }

        private static void AutoSizeColumns(ISheet workSheet, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
                workSheet.AutoSizeColumn(i);
        }

        private static ICellStyle GetTitleStyle(IWorkbook workbook)
        {
            IFont titleFont = workbook.CreateFont();
            titleFont.FontName = "Segoe UI";
            titleFont.FontHeightInPoints = 18;

            ICellStyle titleStyle = workbook.CreateCellStyle();
            titleStyle.SetFont(titleFont);
            return titleStyle;
        }

        private static ICellStyle GetTableHeaderStyle(IWorkbook workbook)
        {
            IFont tableHeaderFont = workbook.CreateFont();
            tableHeaderFont.FontName = "Segoe UI";
            tableHeaderFont.FontHeightInPoints = 12;
            tableHeaderFont.IsBold = true;

            ICellStyle tableHeaderStyle = workbook.CreateCellStyle();
            tableHeaderStyle.SetFont(tableHeaderFont);
            tableHeaderStyle.Alignment = HorizontalAlignment.Center;
            return tableHeaderStyle;
        }

        private static ICellStyle GetInfoStyle(IWorkbook workbook)
        {
            IFont infoFont = workbook.CreateFont();
            infoFont.FontName = "Segoe UI";
            infoFont.FontHeightInPoints = 10;

            ICellStyle infoStyle = workbook.CreateCellStyle();
            infoStyle.SetFont(infoFont);
            return infoStyle;
        }

        private static ICellStyle GetCenteredInfoStyle(IWorkbook workbook)
        {
            ICellStyle infoStyle = GetInfoStyle(workbook);
            infoStyle.Alignment = HorizontalAlignment.Center;
            return infoStyle;
        }

        private static ICellStyle GetRightAlignedInfoStyle(IWorkbook workbook)
        {
            ICellStyle infoStyle = GetInfoStyle(workbook);
            infoStyle.Alignment = HorizontalAlignment.Right;
            return infoStyle;
        }

        private static ICellStyle GetBoldInfoFont(IWorkbook workbook)
        {
            IFont infoFont = workbook.CreateFont();
            infoFont.FontName = "Segoe UI";
            infoFont.FontHeightInPoints = 10;
            infoFont.IsBold = true;

            ICellStyle boldInfoStyle = workbook.CreateCellStyle();
            boldInfoStyle.SetFont(infoFont);
            return boldInfoStyle;
        }

        private static ICellStyle GetRightAlignedBoldInfoFont(IWorkbook workbook)
        {            
            ICellStyle boldInfoStyle = GetBoldInfoFont(workbook);
            boldInfoStyle.Alignment = HorizontalAlignment.Right;
            return boldInfoStyle;
        }
    }
}
