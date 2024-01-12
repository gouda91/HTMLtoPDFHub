using iText.Kernel.Events;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;

namespace HTMLtoPDFHub.Services
{
    public class PageNumberEventHandler : IEventHandler
    {
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();

            int pageNumber = pdfDoc.GetPageNumber(page);
            int totalPages = pdfDoc.GetNumberOfPages();

            // Create a Paragraph with the page number and total pages
            Paragraph footer = new Paragraph()
                .Add("Page " + pageNumber + " of " + totalPages)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(10)
                .SetFontColor(DeviceRgb.BLACK);

            // Calculate the X position for right alignment with a 20-point margin from the right edge
            float xPosition = page.GetPageSize().GetWidth() - 20;

            // Add the footer to the page, aligned to the right
            new Canvas(page, page.GetPageSize())
                .ShowTextAligned(footer, xPosition, 20, TextAlignment.RIGHT);
        }
    }
}

