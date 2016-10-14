using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace ayo.ProcessPDF
{
    public class PdfParser
    {
        public static string ExtractTextFromPdf(string path)
        {
            using (var reader = new PdfReader(path))
            {
                var text = new StringBuilder();

                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString();
            }
        }
    }
}