using System;
using System.Collections.Generic;
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
            var dt = new DataTable();
            dt.Columns.Add("Col 1");
            dt.Columns.Add("Col 2");

            dt.AcceptChanges();

            var row = dt.NewRow();
            row[0] = "Value 1";
            row[1] = "Value 2";

            dt.Rows.Add(row);
            row.AcceptChanges();

            var row2 = dt.NewRow();
            row2[0] = "Value 3";
            row2[1] = "Value 4";

            dt.Rows.Add(row2);
            row2.AcceptChanges();

            dt.AcceptChanges();

            var rManager = new ReportManager();

            var data = rManager.BuildReport(new RecordEntryReportInfo("2017-12-31", "No siemka! :)", dt));

            using (var fs = File.Open(@"D:\itext.pdf", FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
            }

            Assert.AreNotEqual(data, null);

//            var gen = new RdlGenerator();
//            var columns = new List<string>();
//            foreach (DataColumn dataColumn in dt.Columns)
//            {
//                columns.Add(dataColumn.ColumnName);
//            }
//
//            gen.AllFields = columns;
//            gen.SelectedFields = columns;
//            gen.WriteXml(new FileStream(@"D:\test.rdlc", FileMode.OpenOrCreate));

            //rManager.BuildRdlc(dt, "TwojaStara", @"D:\test.rdlc");

            //var done = File.Exists();

            //Assert.IsTrue(done);
        }
    }
}
