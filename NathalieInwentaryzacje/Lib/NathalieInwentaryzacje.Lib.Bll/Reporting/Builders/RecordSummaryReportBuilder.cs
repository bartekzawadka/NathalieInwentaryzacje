using iTextSharp.text;
using iTextSharp.text.pdf;
using LiczbyNaSlowaNET;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary;

namespace NathalieInwentaryzacje.Lib.Bll.Reporting.Builders
{
    internal class RecordSummaryReportBuilder : TypedReportBuilder<RecordSummaryInfo>
    {
        protected override void FillInMainPage(RecordSummaryInfo data, Document document)
        {
            if (data?.TotalsDataset?.Rows == null || data.TotalsDataset?.Rows.Count == 0)
                return;

            var dataTable = new PdfPTable(4)
            {
                WidthPercentage = 100,
                SpacingAfter = 25f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var cellFont = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 12, Font.NORMAL);
            var cellBoldFont = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 12, Font.BOLD);
            const float dataCellRowHeight = 24f;

            var widths = new[]
            {
                1f, 5f, 3f, 3f
            };

            for (var i = 0; i < data.TotalsDataset.Rows.Count; i++)
            {
                AddTableCell(dataTable, (i + 1).ToString(), cellFont, 1, dataCellRowHeight, 0);
                AddTableCell(dataTable, data.TotalsDataset.Rows[i].Name, cellFont, 0, dataCellRowHeight, 0);
                AddTableCell(dataTable, data.TotalsDataset.Rows[i].AdditionalInfo, cellFont, 1, dataCellRowHeight, 0);
                AddTableCell(dataTable, data.TotalsDataset.Rows[i].Value.ToString("C"), cellFont, 2, dataCellRowHeight, 0);
            }

            AddTableCell(dataTable, "", cellBoldFont, 0, dataCellRowHeight, 0);
            AddTableCell(dataTable, "RAZEM:", cellBoldFont, 0, dataCellRowHeight, 0, 2);
            AddTableCell(dataTable, data.TotalsDataset.Sum.ToString("C"), cellBoldFont, 2, dataCellRowHeight, 0);

            dataTable.SetWidths(widths);
            document.Add(dataTable);

            var totalPhraseTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingAfter = 25f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            AddTableCell(totalPhraseTable, "SŁOWNIE:", cellBoldFont, 0, dataCellRowHeight, 0, left: 10f);

            var phrase = NumberToText.Convert(data.TotalsDataset.Sum, Currency.PLN, true);

            var cell3 = new PdfPCell(new Phrase(phrase, cellFont))
            {
                HorizontalAlignment = 0,
                VerticalAlignment = 5,
                MinimumHeight = dataCellRowHeight,
                PaddingLeft = 10f
            };

            totalPhraseTable.AddCell(cell3);

            document.Add(totalPhraseTable);

            var signaturesTable = new PdfPTable(2)
            {
                WidthPercentage = 50,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            var signatureWidths = new[]
            {
                1f, 2f
            };

            const float signaturesRowHeight = 35f;

            AddTableCell(signaturesTable, "Sporządził:", cellFont, 2, signaturesRowHeight, 0, right: 5f);
            AddTableCell(signaturesTable, "", cellFont, 1, signaturesRowHeight, 0);
            AddTableCell(signaturesTable, "Zatwierdził:", cellFont, 2, signaturesRowHeight, 0, right: 5f);
            AddTableCell(signaturesTable, "", cellFont, 1, signaturesRowHeight, 0);

            signaturesTable.SetWidths(signatureWidths);

            document.Add(signaturesTable);
        }

        protected override void CreateHeader(RecordSummaryInfo data, Document document)
        {
            var table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingAfter = 40f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            AddHeaderTableCell(table, "ZESTAWIENIE INWENTARYZACJI", 0, 1, 1, 0, 1, left: 10f);

            AddHeaderTableCell(table, "STAN NA DZIEŃ: " + data.RecordDate.ToRecordDateString(), 2, 1, 0, 1, 1, right: 10f);

            AddHeaderTableCell(table, "WYROBY HANDLOWE I MATERIAŁY", 1, 1, 1, 1, 0, 2);

            document.Add(table);
        }

        private static void AddHeaderTableCell(PdfPTable table, string value, int horizontalAlignment,
            int borderWidth, int borderWidthLeft, int borderWidthRight, int borderWidthTop, int colspan = 1, float? left = null, float? right = null)
        {
            const float fixedHeight = 30f;
            var font = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 14, Font.BOLD);

            var cell3 = new PdfPCell(new Phrase(value, font))
            {
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = 5,
                BorderWidth = borderWidth,
                BorderWidthBottom = 1,
                BorderWidthLeft = borderWidthLeft,
                BorderWidthRight = borderWidthRight,
                BorderWidthTop = borderWidthTop,
                FixedHeight = fixedHeight,
                Colspan = colspan
            };

            if (left != null)
                cell3.PaddingLeft = left.Value;

            if (right != null)
                cell3.PaddingRight = right.Value;

            table.AddCell(cell3);
        }
    }
}
