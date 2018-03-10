using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NathalieInwentaryzacje.Lib.Bll.Reporting.Builders
{
    internal abstract class TypedReportBuilder<T>
    {
        protected static readonly string ArialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");

        public byte[] BuildReport(T data, bool addPageNumber)
        {
            byte[] buff;

            var document = new Document(PageSize.A4);
            using (var ms = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(document, ms);
                if(addPageNumber)
                    writer.PageEvent = new PdfFooter();
                document.Open();

                CreateHeader(data, document);
                FillInMainPage(data, document);

                document.Close();

                buff = ms.ToArray();
            }

            return buff;
        }

        protected abstract void FillInMainPage(T data, Document document);

        protected abstract void CreateHeader(T data, Document document);

        protected virtual void AddTableCell(PdfPTable table, string value, Font font, int horizontalAlignment, Single height,
            int borderWidth, int colspan = 1, float? left = null, float? right = null)
        {
            var cell = new PdfPCell(new Phrase(value, font))
            {
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = 5,
                Colspan = colspan
            };
            if (height > 0)
                cell.FixedHeight = height;
            if (borderWidth > 0)
                cell.BorderWidth = borderWidth;

            if (left != null)
                cell.PaddingLeft = left.Value;

            if (right != null)
                cell.PaddingRight = right.Value;

            table.AddCell(cell);
        }
    }
}
