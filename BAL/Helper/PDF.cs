// using DAL.Models;
// using iText.IO.Font.Constants;
// using iText.IO.Image;
// using iText.Kernel.Colors;
// using iText.Kernel.Font;
// using iText.Kernel.Pdf;
// using iText.Layout;
// using iText.Layout.Borders;
// using iText.Layout.Element;
// using iText.Layout.Properties;
// using Pizzashop.DAL.ViewModels;
// using Table = iText.Layout.Element.Table;

// namespace BAL.Helper;

// public class PDF
// {

//     public byte[] PDF(Order model)
//     {
//         using (MemoryStream ms = new MemoryStream())
//         {
//             using (PdfWriter writer = new PdfWriter(ms))
//             {
//                 using(PdfDocument pdf = new PdfDocument(writer))
//                 {
//                     Document document = new Document(pdf);
//                     PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
//                     PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
//                     DeviceRgb customBlue = new DeviceRgb(0, 102, 167);
//                     DeviceRgb customBlueNew = new DeviceRgb(153, 204, 255); 

//                     document.SetMargins(20, 20, 20, 20);

//                     #region Page Header
//                     var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logos", "pizzashop_logo.png");
//                     Image image = new Image(ImageDataFactory.Create(imagePath))
//                         .SetWidth(65) 
//                         .SetHeight(52) 
//                         .SetHorizontalAlignment(HorizontalAlignment.CENTER);

//                     // Create the paragraph for text
//                     Paragraph orderDetails = new Paragraph("PIZZASHOP")
//                         .SetFont(boldFont)  
//                         .SetFontSize(28)
//                         .SetFontColor(customBlue) 
//                         .SetTextAlignment(TextAlignment.CENTER);

//                     Table tableStart = new Table(2);
//                     tableStart.SetHorizontalAlignment(HorizontalAlignment.CENTER);

//                     Cell imageCell = new Cell()
//                         .Add(image)
//                         .SetPaddingRight(10)
//                         .SetTextAlignment(TextAlignment.RIGHT)
//                         .SetVerticalAlignment(VerticalAlignment.MIDDLE)
//                         .SetBorder(Border.NO_BORDER);

//                     Cell textCell = new Cell()
//                         .Add(orderDetails)
//                         .SetTextAlignment(TextAlignment.CENTER)
//                         .SetVerticalAlignment(VerticalAlignment.MIDDLE)
//                         .SetBorder(Border.NO_BORDER);

//                     // Add cells to the table
//                     tableStart.AddCell(imageCell);
//                     tableStart.AddCell(textCell);

//                     tableStart.SetMarginBottom(20);

//                     // Add table to the document
//                     document.Add(tableStart);

//                     #endregion

//                      #region Page Details
//                     Table table = new Table(3).SetWidth(UnitValue.CreatePercentValue(85)); // Ensuring full width usage
//                     Table leftTable = new Table(1).UseAllAvailableWidth();
//                     Table rightTable = new Table(1).UseAllAvailableWidth();

//                     leftTable.AddCell(new Cell().Add(new Paragraph($"Customer Details").SetFont(boldFont).SetFontSize(15))
//                         .SetBorder(Border.NO_BORDER).SetFontColor(customBlue).SetPaddingTop(0).SetPaddingLeft(0));
//                     leftTable.AddCell(new Cell().Add(new Paragraph($"Name: {model.Customer?.Customername}"))
//                         .SetBorder(Border.NO_BORDER).SetPadding(0).SetMargin(0));
//                     leftTable.AddCell(new Cell().Add(new Paragraph($"Mob: {model.Customer?.Phonenumber}"))
//                         .SetBorder(Border.NO_BORDER).SetPadding(0).SetMargin(0));

//                     rightTable.AddCell(new Cell().Add(new Paragraph($"Order Details").SetFont(boldFont).SetFontSize(15))
//                         .SetBorder(Border.NO_BORDER).SetFontColor(customBlue).SetPaddingTop(0).SetPaddingLeft(0));
//                     rightTable.AddCell(new Cell().Add(new Paragraph($"Invoice Number: #DOM{model.InvoiceNo}"))
//                         .SetBorder(Border.NO_BORDER).SetPadding(0).SetMargin(0));
//                     rightTable.AddCell(new Cell().Add(new Paragraph($"Date: {model.CreatedDate}"))
//                         .SetBorder(Border.NO_BORDER).SetPadding(0).SetMargin(0));
//                     rightTable.AddCell(new Cell().Add(new Paragraph($"Section: {model?.OrderTables.}"))
//                         .SetBorder(Border.NO_BORDER).SetPadding(0).SetMargin(0));
//                     rightTable.AddCell(new Cell().Add(new Paragraph($"Table: {string.Join(", ", model.OrderTables.Tab!)}"))
//                         .SetBorder(Border.NO_BORDER).SetPadding(0).SetMargin(0));

//                     table.AddCell(new Cell().Add(leftTable).SetBorder(Border.NO_BORDER));
//                     table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(UnitValue.CreatePercentValue(30))); 
//                     table.AddCell(new Cell().Add(rightTable).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT).SetPaddingLeft(30));
//                     table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
//                     document.Add(table);

//                     #endregion

//                     #region Table
//                     table = new Table(new float[] { 1, 3, 1, 1, 1 }).SetWidth(UnitValue.CreatePercentValue(85)).SetMarginTop(20);
//                     table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
//                     table.AddHeaderCell(CreateStyledCell("Sr.No", customBlue, isLeft: true));
//                     table.AddHeaderCell(CreateStyledCell("Item", customBlue, isLeft: true));
//                     table.AddHeaderCell(CreateStyledCell("Quantity", customBlue));
//                     table.AddHeaderCell(CreateStyledCell("Price", customBlue));
//                     table.AddHeaderCell(CreateStyledCell("Total", customBlue, isRight: true));

//                     for (int i = 0; i < model.OrderItems?.Count; i++)
//                     {
//                         var dish = model.OrderItems.[i];
//                         table.AddCell(CreateCell((i + 1).ToString(), bold: true, isLeft: true));
//                         table.AddCell(CreateCell(dish.ItemName, bold: true, isLeft: true));
//                         table.AddCell(CreateCell(dish.Quantity.ToString(), bold: true));
//                         table.AddCell(CreateCell($"₹{dish.Rate:F2}", bold: true));
//                         table.AddCell(CreateCell($"₹{dish.TotalAmount:F2}", isRight: true, bold: true));

//                         foreach (var mod in dish.AddOns ?? new List<ModifierItemViewModel>())
//                         {
//                             table.AddCell(CreateCell("", isLeft:true)); // Empty for Sr.No
//                             table.AddCell(CreateCell($"→ {mod.Name}", false, isLeft: true));
//                             table.AddCell(CreateCell(mod.Quantity.ToString()));
//                             table.AddCell(CreateCell($"₹{mod.Rate:F2}"));
//                             table.AddCell(CreateCell($"₹{mod.Quantity*mod.Rate:F2}", isRight: true));
//                         }

//                         table.AddCell(new Cell(1, 5).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(customBlueNew, 1)).SetMarginTop(3));
//                     }

//                     document.Add(table);
                
//                     #endregion


//                 }
//             }
//         }
//     }

//    private Cell CreateCell(string text, bool bold = false, bool italic = false, bool isLeft = false, bool isRight = false)
//     {
//         PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
//         if (bold) font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

//         Paragraph paragraph = new Paragraph(text).SetFont(font).SetFontSize(10);
//         if (italic) paragraph.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE));

//         Cell cell = new Cell().Add(paragraph);
//         cell.SetPadding(5);
//         cell.SetBorder(Border.NO_BORDER);
//         cell.SetTextAlignment(TextAlignment.CENTER);
//         cell.SetPaddingTop(3);
//         cell.SetPaddingBottom(0);
//         cell.SetMarginTop(0);
//         cell.SetMarginBottom(0);

//         cell.SetMarginTop(10);

//         if(isLeft) cell.SetTextAlignment(TextAlignment.LEFT);
//         if(isRight) cell.SetTextAlignment(TextAlignment.RIGHT);
//         if(isLeft) cell.SetPaddingLeft(10);
//         return cell;

//     }
//     private static Cell CreateStyledCell(string text, DeviceRgb bgColor, bool isLeft = false, bool isRight = false)
//     {
//         Paragraph paragraph = new Paragraph(text)
//                 .SetFontColor(ColorConstants.WHITE) // White text
//                 .SetTextAlignment(TextAlignment.CENTER); 

//         if(isLeft) paragraph.SetTextAlignment(TextAlignment.LEFT); 
//         if(isRight) paragraph.SetTextAlignment(TextAlignment.RIGHT);
        
//         Cell cell = new Cell()
//             .Add(paragraph)
//             .SetBackgroundColor(bgColor) // Custom background color
//             .SetPadding(5).SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetBorder(Border.NO_BORDER); 

//         if(isLeft)
//         {
//             cell.SetPaddingLeft(10);
//             cell.SetMarginLeft(0);
//         }

//         if(isRight) cell.SetPaddingRight(7);



//         return cell;  
//     }
// }
