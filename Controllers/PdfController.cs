using Microsoft.AspNetCore.Mvc;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using HTMLtoPDFHub.Services;
using iText.Kernel.Events;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace HTMLtoPDFHub.Controllers
{
    [Route("api")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpPost]
        [Route("convert")]
        public IActionResult ConvertHtmlToPdf([FromBody] string htmlContent)
        {
            htmlContent = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <style>\r\n        /* Your CSS for styling page numbers goes here */\r\n        body {\r\n            counter-reset: page;\r\n        }\r\n\r\n        .page-number::before {\r\n            counter-increment: page;\r\n            content: \"Page \" counter(page);\r\n        }\r\n\r\n        .page-number {\r\n            position: absolute;\r\n            bottom: 10px;\r\n            right: 10px;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div style=\"height: 700px; background-color: red;\"></div>\r\n    <div style=\"height: 700px; background-color: red;\"></div>\r\n    <div style=\"height: 700px; background-color: red;\"></div>\r\n     <img style=\"width:40px\" src=\"data:image/png;base64,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\" alt=\"Butterfly\">\r\n\r\n    <!-- Page number placeholder -->\r\n    <div class=\"page-number\"></div>\r\n</body>\r\n</html>\r\n";
            try
            {
                if (string.IsNullOrEmpty(htmlContent))
                {
                    return BadRequest("HTML content is missing.");
                }

                byte[] pdfBytes;

                // First, convert HTML to PDF and store it in a byte array

                using (var stream = new MemoryStream())
                {
                    HtmlConverter.ConvertToPdf(htmlContent, stream);
                    pdfBytes = stream.ToArray();
                }

                // Now create a new MemoryStream for the final output
                using (var finalStream = new MemoryStream())
                {
                    // Initialize PdfReader with the byte array
                    using (var reader = new PdfReader(new MemoryStream(pdfBytes)))
                    {
                        // Initialize PdfWriter with the final stream
                        using (var writer = new PdfWriter(finalStream))
                        {
                            // Create a new PdfDocument
                            using (var pdf = new PdfDocument(reader, writer))
                            {
                                // Register the event handler for adding page numbers
                                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberEventHandler());
                            }
                        }
                    }

                    // The MemoryStream now contains the PDF data, reset its position to the beginning

                    // Return the PDF as a FileResult
                    return File(finalStream.ToArray(), "application/pdf", "converted.pdf");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet]
        [Route("")]
        public IActionResult HealthCheck()
        {
            return Ok("API is running");
        }
    }
}

