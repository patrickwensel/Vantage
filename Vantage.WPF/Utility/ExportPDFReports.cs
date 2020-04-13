using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vantage.Common.Models;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Models;

namespace Vantage.WPF.Utility
{
    public static class ExportPDFReports
    {
        private static iTextSharp.text.Font titleFont = FontFactory.GetFont("Segoe UI", 18, new iTextSharp.text.BaseColor(System.Drawing.Color.Black));
        private static iTextSharp.text.Font tableHeaderFont = FontFactory.GetFont("Segoe UI", 12, Font.BOLD, new iTextSharp.text.BaseColor(System.Drawing.Color.Black));
        private static iTextSharp.text.Font infoFont = FontFactory.GetFont("Segoe UI", 10, new iTextSharp.text.BaseColor(System.Drawing.Color.Black));
        private static iTextSharp.text.Font boldInfoFont = FontFactory.GetFont("Segoe UI", 10, Font.BOLD, new iTextSharp.text.BaseColor(System.Drawing.Color.Black));
        private static string DateFormat = "dd-MMM-yyyy h:mm tt";

        public static void TrainingReport(IList<SelectableDriver> drivers, string absoluteFileName)
        {
            using (FileStream fs = new FileStream(absoluteFileName, FileMode.OpenOrCreate))
            {
                // Create an instance of the document class which represents the PDF document itself.  
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);

                // Create an instance to the PDF file by creating an instance of the PDF   
                // Writer class using the document and the filestrem in the constructor.  
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                AddMetaInformationForDocument(document, "Driver Training Report", "Training Report");

                Phrase titlePhrase = new Phrase();
                Chunk reportTitle = new Chunk("Training Report", titleFont);
                Chunk dateTime = new Chunk(DateTime.Now.ToString(DateFormat), infoFont);
                Chunk centerSpace = new Chunk(new VerticalPositionMark());

                titlePhrase.Add(reportTitle);
                titlePhrase.Add(centerSpace);
                titlePhrase.Add(dateTime);

                try 
                {
                    document.Open();

                    document.Add(titlePhrase);

                    PdfPTable pdfPTable = new PdfPTable(6);
                    pdfPTable.WidthPercentage = 100f;
                    pdfPTable.PaddingTop = 40;

                    //Adding Headers Here.
                    pdfPTable.AddCell(GetHeaderCell("Last Name", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("First Name", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Group", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Lessons Completed", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Total Lessons", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Course Completed?", tableHeaderFont));
                    // Headers Completed...

                    // Adding data now.
                    foreach (Driver driver in drivers)
                    {
                        int completedLessonCount = driver.GroupedAttemptsByLessons.Count(x => x.IsComplete);
                        int totalLessonCount = driver.GroupedAttemptsByLessons.Count();
                        string couserCompleted = completedLessonCount == totalLessonCount ? "Yes" : "No";
                        pdfPTable.AddCell(GetContentCell(driver.LastName, infoFont, Element.ALIGN_LEFT));
                        pdfPTable.AddCell(GetContentCell(driver.FirstName, infoFont, Element.ALIGN_LEFT));
                        pdfPTable.AddCell(GetContentCell(driver.Group.Name, infoFont, Element.ALIGN_LEFT));
                        pdfPTable.AddCell(GetContentCell(completedLessonCount.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(totalLessonCount.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(couserCompleted, infoFont, Element.ALIGN_LEFT));
                    }

                    document.Add(pdfPTable);
                }
                catch(Exception ex)
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

        public static void IndividualDriversReport(SelectableDriver driver, string absoluteFileName)
        {
            using (FileStream fs = new FileStream(absoluteFileName, FileMode.OpenOrCreate))
            {
                // Create an instance of the document class which represents the PDF document itself.  
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);

                // Create an instance to the PDF file by creating an instance of the PDF   
                // Writer class using the document and the filestrem in the constructor.  
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                AddMetaInformationForDocument(document, "Individual Driver Report", "Indvidual Driver Report");

                Phrase titlePhrase = new Phrase();
                Chunk reportTitle = new Chunk("Individual Driver Report", titleFont);
                Chunk dateTime = new Chunk(DateTime.Now.ToString(DateFormat), infoFont);
                Chunk centerSpace = new Chunk(new VerticalPositionMark());

                titlePhrase.Add(reportTitle);
                titlePhrase.Add(centerSpace);
                titlePhrase.Add(dateTime);

                Paragraph driverName = new Paragraph($"{driver.LastName}, {driver.FirstName}", tableHeaderFont);
                driverName.Alignment = Element.ALIGN_CENTER;
                driverName.Add(new Phrase(Environment.NewLine));
                driverName.Add(new Phrase(Environment.NewLine));

                try
                {
                    document.Open();

                    document.Add(titlePhrase);
                    document.Add(driverName);

                    PdfPTable pdfPTable = new PdfPTable(5);
                    pdfPTable.WidthPercentage = 100f;

                    //Adding Headers Here.
                    pdfPTable.AddCell(GetHeaderCell("Lesson", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("High Score", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("# of Attempts", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Total Time (In Mins)", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Date Completed", tableHeaderFont));
                    // Headers Completed...

                    // Adding data now.
                    foreach (GroupedAttemptsByLesson attempt in driver.GroupedAttemptsByLessons)
                    {                       
                        pdfPTable.AddCell(GetContentCell(attempt.Lesson.Name, infoFont, Element.ALIGN_LEFT));
                        pdfPTable.AddCell(GetContentCell(attempt.HighScore.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalAttempts.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalTimes.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.DateCompleted != null ? attempt.DateCompleted?.ToString("dd-MMM-yyyy") : string.Empty, infoFont, Element.ALIGN_RIGHT));
                    }

                    document.Add(pdfPTable);
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
            using (FileStream fs = new FileStream(absoluteFileName, FileMode.OpenOrCreate))
            {
                // Create an instance of the document class which represents the PDF document itself.  
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);

                // Create an instance to the PDF file by creating an instance of the PDF   
                // Writer class using the document and the filestrem in the constructor.  
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                AddMetaInformationForDocument(document, "Detailed Lesson Report", "Driver's Detailed Lesson Report");
                
                Phrase titlePhrase = new Phrase();
                Chunk reportTitle = new Chunk("Detailed Lesson Report", titleFont);
                Chunk dateTime = new Chunk(DateTime.Now.ToString(DateFormat), infoFont);
                Chunk centerSpace = new Chunk(new VerticalPositionMark());

                titlePhrase.Add(reportTitle);
                titlePhrase.Add(centerSpace);
                titlePhrase.Add(dateTime);

                Paragraph driverName = new Paragraph($"{driver.LastName}, {driver.FirstName}", tableHeaderFont);
                driverName.Alignment = Element.ALIGN_CENTER;
                driverName.Add(new Phrase(Environment.NewLine));
                driverName.Add(new Phrase(Environment.NewLine));

                try
                {
                    document.Open();

                    document.Add(titlePhrase);
                    document.Add(driverName);

                    PdfPTable pdfPTable = new PdfPTable(5);
                    pdfPTable.WidthPercentage = 100f;

                    //Adding Headers Here.
                    pdfPTable.AddCell(GetHeaderCell("Lesson", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("High Score", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("# of Attempts", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Total Time (In Mins)", tableHeaderFont));
                    pdfPTable.AddCell(GetHeaderCell("Date Completed", tableHeaderFont));
                    // Headers Completed...

                    // Adding data now.
                    foreach (GroupedAttemptsByLesson attempt in driver.GroupedAttemptsByLessons)
                    {
                        pdfPTable.AddCell(GetContentCell(attempt.Lesson.Name, infoFont, Element.ALIGN_LEFT));
                        pdfPTable.AddCell(GetContentCell(attempt.HighScore.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalAttempts.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.TotalTimes.ToString(), infoFont, Element.ALIGN_RIGHT));
                        pdfPTable.AddCell(GetContentCell(attempt.DateCompleted != null ? attempt.DateCompleted?.ToString("dd-MMM-yyyy") : string.Empty, infoFont, Element.ALIGN_RIGHT));

                        if (attempt.GroupedInfractions == null || attempt.GroupedInfractions.Count == 0)
                            continue;

                        // Adding Infractions now.
                        PdfPTable infractionsTable = new PdfPTable(3);
                        infractionsTable.HorizontalAlignment = Element.ALIGN_CENTER;

                        //Adding infractions Headers Here.
                        infractionsTable.AddCell(GetHeaderCell("Infractions", tableHeaderFont));
                        infractionsTable.AddCell(GetHeaderCell("# of Occurances", tableHeaderFont));
                        infractionsTable.AddCell(GetHeaderCell("Points Deducted", tableHeaderFont));
                        // Headers Completed...

                        foreach(GroupedInfractions groupedInfractions in attempt.GroupedInfractions)
                        {
                            infractionsTable.AddCell(GetContentCell(groupedInfractions.Infraction.Name, infoFont, Element.ALIGN_LEFT));
                            infractionsTable.AddCell(GetContentCell(groupedInfractions.Occurances.ToString(), infoFont, Element.ALIGN_RIGHT));
                            infractionsTable.AddCell(GetContentCell(groupedInfractions.Deduction.ToString(), infoFont, Element.ALIGN_RIGHT));
                        }

                        // Adding Total
                        infractionsTable.AddCell(GetContentCell(string.Empty, infoFont, Element.ALIGN_LEFT));
                        infractionsTable.AddCell(GetContentCell("Total", boldInfoFont, Element.ALIGN_LEFT));
                        infractionsTable.AddCell(GetContentCell(attempt.TotalDeduction.ToString(), boldInfoFont, Element.ALIGN_RIGHT));

                        PdfPCell infractionTableCell = new PdfPCell(infractionsTable);
                        infractionTableCell.Colspan = 5;
                        infractionTableCell.Padding = 25;
                                               
                        pdfPTable.AddCell(infractionTableCell);
                    }

                    document.Add(pdfPTable);
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

        private static void AddMetaInformationForDocument(Document document, string subject, string title)
        {
            document.AddAuthor("Vantage Driver Management");
            document.AddCreator("Vantage Driver Management App");
            document.AddKeywords("Vantage Driver Management PDF Reports");
            document.AddSubject(subject);
            document.AddTitle(title);
        }

        private static PdfPCell GetHeaderCell(string header, Font tableHeaderFont)
        {            
            PdfPCell headerCell = new PdfPCell(new Phrase(header, tableHeaderFont));
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.VerticalAlignment = Element.ALIGN_CENTER;
            return headerCell;
        }

        private static PdfPCell GetContentCell(string content, Font contentFont, int alignment = Element.ALIGN_LEFT)
        {            
            PdfPCell contentCell = new PdfPCell(new Phrase(content, contentFont));
            contentCell.HorizontalAlignment = alignment;
            contentCell.VerticalAlignment = Element.ALIGN_CENTER;
            return contentCell;
        }
    }
}
