using System;
using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Reporting;

namespace NathalieInwentaryzacje.Tests.Reporting
{
    [TestClass]
    public class ReportManagerTests
    {
        [TestMethod]
        public void BuildDynReport()
        {
            var dt = GenerateData();

            var rManager = new ReportManager();

            var data = rManager.BuildReport(new RecordEntryReportInfo("2017-12-31", "No siemka! :)", dt));

//            using (var fs = File.Open(@"D:\itext.pdf", FileMode.OpenOrCreate, FileAccess.Write))
//            {
//                fs.Write(data, 0, data.Length);
//            }

            Assert.AreNotEqual(data, null);
        }

        private DataTable GenerateData()
        {
            var dt = new DataTable();
            dt.Columns.Add("Nazwa");
            dt.Columns.Add("Wartość");

            dt.AcceptChanges();

            var rowsNum = 150;

            var rand = new Random(100);

            for (var i = 0; i < rowsNum; i++)
            {
                var row = dt.NewRow();
                row[0] = Guid.NewGuid();
                row[1] = rand.Next();
                dt.Rows.Add(row);
                row.AcceptChanges();
            }

            dt.AcceptChanges();

            return dt;
        }
    }
}
