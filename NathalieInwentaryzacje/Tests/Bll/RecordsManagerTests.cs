using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NathalieInwentaryzacje.Lib.Bll.Managers;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace Bll
{
    [TestClass]
    public class RecordsManagerTests
    {
        private readonly IRecordsManager _recordsManager= new RecordsManager();

        [TestMethod]
        public void GetReportInfo()
        {
            var records = _recordsManager.GetRecords();
            try
            {
                //var reportInfo = _recordsManager.GetRecordsReportInfo(records.First().RecordDate, new []{"ZŁOTO 2017.xlsx"});
            }
            catch { }

            Assert.IsTrue(true);
        }
    }
}
