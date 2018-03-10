using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAnnex;

namespace NathalieInwentaryzacje.Lib.Bll.Reporting.Builders
{
    internal class RecordAppendixReportBuilder : TypedReportBuilder<RecordAppendixInfo>
    {
        protected override void FillInMainPage(RecordAppendixInfo data, Document document)
        {
            for (var i = 0; i < data.SubSets.Count; i++)
            {
                CreateSubSet(data.SubSets[i], document, i < data.SubSets.Count - 1);
            }
        }

        protected override void CreateHeader(RecordAppendixInfo data, Document document)
        {
            var table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingAfter = 20f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var font = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 14, Font.BOLD);

            AddTableCell(table,
                "ZAŁĄCZNIK NR " + data.AppendixNumber + " DO INWENTARYZACJI" + Environment.NewLine + "STAN NA DZIEŃ: " +
                data.RecordDate.ToRecordDateString(), font, 1, 48f, 1);

            document.Add(table);
        }

        private void CreateSubSet(RecordAppendixSubSet subset, Document document, bool setMarginBottom)
        {
            if (subset?.Rows == null || subset.Rows.Count == 0)
                return;

            var font = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 14, Font.BOLD);
            var cellFont = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 12, Font.NORMAL);
            var cellBoldFont = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 12, Font.BOLD);

            var headerTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            AddTableCell(headerTable, subset.Title, font, 1, 48f, 0);

            document.Add(headerTable);

            var widths = new[]
            {
                1f, 4f, 3f
            };

            
            var dataTable = new PdfPTable(3)
            {
                WidthPercentage = 100
            };

            const float dataCellRowHeight = 22f;

            for (var i = 0; i < subset.Rows.Count; i++)
            {
                AddTableCell(dataTable, (i + 1).ToString(), cellFont, 1, dataCellRowHeight, 0);
                AddTableCell(dataTable, subset.Rows[i].Name, cellFont, 0, dataCellRowHeight, 0);
                AddTableCell(dataTable, subset.Rows[i].Value.ToString("C"), cellFont, 2, dataCellRowHeight, 0);
            }


            AddTableCell(dataTable, "", cellBoldFont, 0, dataCellRowHeight, 0);
            AddTableCell(dataTable, "RAZEM:", cellBoldFont, 0, dataCellRowHeight, 0);
            AddTableCell(dataTable, subset.Sum.ToString("C"), cellBoldFont, 2, dataCellRowHeight, 0);

            if (setMarginBottom)
                dataTable.SpacingAfter = 40f;

            dataTable.SetWidths(widths);

            document.Add(dataTable);
        }
    }
}
