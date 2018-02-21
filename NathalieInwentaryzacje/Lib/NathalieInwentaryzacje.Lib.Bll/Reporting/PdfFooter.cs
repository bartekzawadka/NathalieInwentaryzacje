using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NathalieInwentaryzacje.Lib.Bll.Reporting
{
    public class PdfFooter : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            var tabFot = new PdfPTable(new[] {1F})
            {
                TotalWidth = 100F,
                HorizontalAlignment = 2
            };
            var cell = new PdfPCell(new Phrase(document.PageNumber.ToString()))
            {
                HorizontalAlignment = 2,
                BorderWidth = 0
            };
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, document.Right - 100, document.Bottom, writer.DirectContent);
        }
    }
}
