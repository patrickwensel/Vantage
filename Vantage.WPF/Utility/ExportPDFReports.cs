using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Vantage.Common.Models;
using Vantage.WPF.Models;

namespace Vantage.WPF.Utility
{
    public static class ExportPDFReports
    {
        //private static PdfFont segoeUIFont = PdfFontFactory.CreateFont("Segoe UI", false);        
        private static Style titleStyle = new Style().SetFontSize(18);
        private static Style tableHeaderStyle = new Style().SetFontSize(12).SetBold();
        private static Style infoStyle = new Style().SetFontSize(10);
        private static Style boldInfoStyle = infoStyle.SetBold();
        private static string DateFormat = "dd-MMM-yyyy h:mm tt";


        public static void TestTrainingReport()
        {
            // Must have write permissions to the path folder
            PdfWriter writer = new PdfWriter("C:\\Users\\Javed\\Desktop\\demo.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Header
            Paragraph header = new Paragraph("HEADER")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(20);

            // New line
            Paragraph newline = new Paragraph(new Text("\n"));

            document.Add(newline);
            document.Add(header);

            // Add sub-header
            Paragraph subheader = new Paragraph("SUB HEADER")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(15);
            document.Add(subheader);

            // Line separator
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            // Add paragraph1
            Paragraph paragraph1 = new Paragraph("Lorem ipsum " +
               "dolor sit amet, consectetur adipiscing elit, " +
               "sed do eiusmod tempor incididunt ut labore " +
               "et dolore magna aliqua.");
            document.Add(paragraph1);

            // Add image
            //Image img = new Image(ImageDataFactory
            //   .Create(@"..\Images\Cover.png"))
            //   .SetTextAlignment(TextAlignment.CENTER);
            //document.Add(img);

            // Table
            Table table = new Table(2, false);
            Cell cell11 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("State"));
            Cell cell12 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Capital"));

            Cell cell21 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("New York"));
            Cell cell22 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Albany"));

            Cell cell31 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("New Jersey"));
            Cell cell32 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Trenton"));

            Cell cell41 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("California"));
            Cell cell42 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Sacramento"));

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell31);
            table.AddCell(cell32);
            table.AddCell(cell41);
            table.AddCell(cell42);

            document.Add(newline);
            document.Add(table);

            // Hyper link
            Link link = new Link("click here",
               PdfAction.CreateURI("https://www.google.com"));
            Paragraph hyperLink = new Paragraph("Please ")
               .Add(link.SetBold().SetUnderline()
               .SetItalic().SetFontColor(ColorConstants.BLUE))
               .Add(" to go www.google.com.");

            document.Add(newline);
            document.Add(hyperLink);

            // Page numbers
            int n = pdf.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(String
                   .Format("page" + i + " of " + n)),
                   559, 806, i, TextAlignment.RIGHT,
                   VerticalAlignment.TOP, 0);
            }

            // Close document
            document.Close();
        }

        public static void TrainingReport(IList<SelectableDriver> drivers, string absoluteFileName)
        {
            //using (FileStream fs = new FileStream(absoluteFileName, FileMode.OpenOrCreate))
            using (PdfWriter writer = new PdfWriter(absoluteFileName))
            {
                PdfDocument pdf = new PdfDocument(writer);

                // Create an instance of the document class which represents the PDF document itself.                
                Document document = new Document(pdf, PageSize.A4, false);

                AddMetaInformationForDocument(pdf, "Driver Training Report", "Training Report");

                Paragraph titlePhrase = new Paragraph();
                Text reportTitle = new Text("Training Report")
                    .AddStyle(titleStyle);
                Text dateTime = new Text($"\n{DateTime.Now.ToString(DateFormat)}").AddStyle(infoStyle);
                
                titlePhrase.Add(reportTitle);
                titlePhrase.Add(dateTime);

                try
                {
                    document.Add(titlePhrase);

                    Table pdfPTable = new Table(6);
                    pdfPTable.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(95f));
                    pdfPTable.SetPaddingTop(40f);

                    //Adding Headers Here.
                    pdfPTable.AddCell(GetHeaderCell("Last Name"));
                    pdfPTable.AddCell(GetHeaderCell("First Name"));
                    pdfPTable.AddCell(GetHeaderCell("Group"));
                    pdfPTable.AddCell(GetHeaderCell("Lesson(s) Completed"));
                    pdfPTable.AddCell(GetHeaderCell("Total Lessons"));
                    pdfPTable.AddCell(GetHeaderCell("Course Completed?"));
                    // Headers Completed...

                    // Adding data now.
                    foreach (Driver driver in drivers)
                    {
                        int completedLessonCount = driver.GroupedAttemptsByLessons.Count(x => x.IsComplete);
                        int totalLessonCount = driver.GroupedAttemptsByLessons.Count();
                        string couserCompleted = completedLessonCount == totalLessonCount ? "Yes" : "No";
                        pdfPTable.AddCell(GetContentCell(driver.LastName));
                        pdfPTable.AddCell(GetContentCell(driver.FirstName));                        
                        pdfPTable.AddCell(GetContentCell(driver.Group?.Name));
                        pdfPTable.AddCell(GetContentCell(completedLessonCount.ToString(), iText.Layout.Properties.TextAlignment.CENTER));
                        pdfPTable.AddCell(GetContentCell(totalLessonCount.ToString(), iText.Layout.Properties.TextAlignment.CENTER));
                        pdfPTable.AddCell(GetContentCell(couserCompleted, iText.Layout.Properties.TextAlignment.CENTER));
                    }

                    document.Add(pdfPTable);
                    AddPageNumbers(pdf, document);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    document.Close();
                }
            }
        }

        public static void IndividualDriversReport(SelectableDriver driver, string absoluteFileName)
        {
            using (PdfWriter writer = new PdfWriter(absoluteFileName))
            {
                PdfDocument pdf = new PdfDocument(writer);

                // Create an instance of the document class which represents the PDF document itself.                
                Document document = new Document(pdf, PageSize.A4);                                

                AddMetaInformationForDocument(pdf, "Individual Driver Report", "Indvidual Driver Report");

                Paragraph titlePhrase = new Paragraph();
                Text reportTitle = new Text("Individual Driver Report").AddStyle(titleStyle);
                Text dateTime = new Text($"\n{DateTime.Now.ToString(DateFormat)}").AddStyle(infoStyle);

                titlePhrase.Add(reportTitle);
                titlePhrase.Add(dateTime);

                Paragraph driverName = new Paragraph();
                Text driverNameText = new Text($"{driver.LastName}, {driver.FirstName}\n\n").AddStyle(titleStyle);
                driverName.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                driverName.Add(driverNameText);

                try
                {
                    document.Add(titlePhrase);
                    document.Add(driverName);

                    Table pdfPTable = new Table(5);
                    pdfPTable.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100f));

                    //Adding Headers Here.
                    pdfPTable.AddCell(GetHeaderCell("Lesson"));
                    pdfPTable.AddCell(GetHeaderCell("High Score"));
                    pdfPTable.AddCell(GetHeaderCell("# of Attempts"));
                    pdfPTable.AddCell(GetHeaderCell("Total Time (m)"));
                    pdfPTable.AddCell(GetHeaderCell("Date Completed"));
                    // Headers Completed...

                    // Adding data now.
                    foreach (GroupedAttemptsByLesson attempt in driver.GroupedAttemptsByLessons)
                    {
                        pdfPTable.AddCell(GetContentCell(attempt.Lesson.Name));
                        pdfPTable.AddCell(GetContentCell(attempt.HighScore.ToString(), iText.Layout.Properties.TextAlignment.CENTER));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalAttempts.ToString(), iText.Layout.Properties.TextAlignment.RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalTimes.ToString(), iText.Layout.Properties.TextAlignment.RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.DateCompleted != null ? attempt.DateCompleted?.ToString("dd-MMM-yyyy") : string.Empty, iText.Layout.Properties.TextAlignment.RIGHT));
                    }

                    document.Add(pdfPTable);
                    AddPageNumbers(pdf, document);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    document.Close();
                    writer.Close();
                }
            }
        }

        public static void DetailedLessonReport(SelectableDriver driver, string absoluteFileName)
        {
            using (PdfWriter writer = new PdfWriter(absoluteFileName))
            {
                PdfDocument pdf = new PdfDocument(writer);

                // Create an instance of the document class which represents the PDF document itself.                
                Document document = new Document(pdf, PageSize.A4);
                
                AddMetaInformationForDocument(pdf, "Detailed Lesson Report", "Driver's Detailed Lesson Report");

                Paragraph titlePhrase = new Paragraph();
                Text reportTitle = new Text("Detailed Lesson Report").AddStyle(titleStyle);
                Text dateTime = new Text($"\n{DateTime.Now.ToString(DateFormat)}").AddStyle(infoStyle);                

                titlePhrase.Add(reportTitle);
                titlePhrase.Add(dateTime);

                Paragraph driverName = new Paragraph();
                Text driverNameText = new Text($"{driver.LastName}, {driver.FirstName}\n\n").AddStyle(titleStyle);
                driverName.SetTextAlignment(TextAlignment.CENTER);
                driverName.Add(driverNameText);

                try
                {                   
                    document.Add(titlePhrase);
                    document.Add(driverName);

                    Table pdfPTable = new Table(5);                    
                    pdfPTable.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100f));

                    //Adding Headers Here.
                    pdfPTable.AddCell(GetHeaderCell("Lesson"));
                    pdfPTable.AddCell(GetHeaderCell("High Score"));
                    pdfPTable.AddCell(GetHeaderCell("# of Attempts"));
                    pdfPTable.AddCell(GetHeaderCell("Total Time (m)"));
                    pdfPTable.AddCell(GetHeaderCell("Date Completed"));
                    // Headers Completed...

                    // Adding data now.
                    foreach (GroupedAttemptsByLesson attempt in driver.GroupedAttemptsByLessons)
                    {
                        pdfPTable.AddCell(GetContentCell(attempt.Lesson.Name));
                        pdfPTable.AddCell(GetContentCell(attempt.HighScore.ToString(), iText.Layout.Properties.TextAlignment.RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalAttempts.ToString(), iText.Layout.Properties.TextAlignment.RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalTimes.ToString(), iText.Layout.Properties.TextAlignment.RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.DateCompleted != null ? attempt.DateCompleted?.ToString("dd-MMM-yyyy") : string.Empty, iText.Layout.Properties.TextAlignment.RIGHT));

                        if (attempt.GroupedInfractions == null || attempt.GroupedInfractions.Count == 0)
                            continue;

                        // Adding Infractions now.
                        Table infractionsTable = new Table(3);
                        infractionsTable.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        //Adding infractions Headers Here.
                        infractionsTable.AddCell(GetHeaderCell("Infractions"));
                        infractionsTable.AddCell(GetHeaderCell("# of Occurrences"));
                        infractionsTable.AddCell(GetHeaderCell("Points Deducted"));
                        // Headers Completed...

                        foreach (GroupedInfractions groupedInfractions in attempt.GroupedInfractions)
                        {
                            infractionsTable.AddCell(GetContentCell(groupedInfractions.Infraction.Name));
                            infractionsTable.AddCell(GetContentCell(groupedInfractions.Occurances.ToString(), iText.Layout.Properties.TextAlignment.RIGHT));
                            infractionsTable.AddCell(GetContentCell(groupedInfractions.Deduction.ToString(), iText.Layout.Properties.TextAlignment.RIGHT));
                        }

                        // Adding Total
                        infractionsTable.AddCell(GetContentCell(string.Empty));
                        infractionsTable.AddCell(GetContentCell("Total"));
                        Cell boldCell = GetContentCell(attempt.TotalDeduction.ToString(), iText.Layout.Properties.TextAlignment.RIGHT);
                        boldCell.AddStyle(boldInfoStyle);
                        infractionsTable.AddCell(boldCell);

                        Cell infractionTableCell = new Cell(1, 5);
                        infractionTableCell.SetPadding(25);
                        infractionTableCell.Add(infractionsTable);

                        pdfPTable.AddCell(infractionTableCell);
                    }

                    document.Add(pdfPTable);
                    AddPageNumbers(pdf, document);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    document.Close();
                    writer.Close();
                }
            }
        }

        private static void AddMetaInformationForDocument(PdfDocument document, string subject, string title)
        {
            PdfDocumentInfo info = document.GetDocumentInfo();            
            info.SetAuthor("Vantage Driver Management");            
            info.SetKeywords("Vantage Driver Management PDF Reports");
            info.SetCreator("Vantage Driver Management App");
            info.SetSubject(subject);
            info.SetTitle(title);
        }

        private static void AddPageNumbers(PdfDocument pdf, Document document)
        {
            // Page numbers
            int n = pdf.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(String
                   .Format("page" + i + " of " + n)),
                   559, 806, i, TextAlignment.RIGHT,
                   VerticalAlignment.TOP, 0);
            }
        }

        private static Cell GetHeaderCell(string header)
        {
            Cell headerCell = new Cell();
            headerCell.Add(new Paragraph(header))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
            return headerCell;
        }

        private static Cell GetContentCell(string content, iText.Layout.Properties.TextAlignment alignment = iText.Layout.Properties.TextAlignment.LEFT)
        {
            if (content == null)
                content = string.Empty;

            Cell contentCell = new Cell();
            contentCell.Add(new Paragraph(content))
                .SetTextAlignment(alignment)
                .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
            return contentCell;
        }
    }
}
