using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NathalieInwentaryzacje.Lib.Bll.Managers;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordSummary;

namespace BllTests
{
    [TestClass]
    public class ReportTests
    {
        [TestMethod]
        public void SummaryReportTest()
        {
            var data = new RecordSummaryInfo
            {
                RecordDate = DateTime.Now,
                TotalsDataset = new RecordsTotalsInfo
                {
                    Rows = new List<RecordTotalRowInfo>
                    {
                        new RecordTotalRowInfo
                        {
                            Name = "Test",
                            Value = 140.35M
                        },
                        new RecordTotalRowInfo
                        {
                            Name = "Wyroby własne z kamieniami",
                            AdditionalInfo = "Szu szu szu",
                            Value = 31.89M
                        },
                        new RecordTotalRowInfo
                        {
                            Name = "Test 3",
                            Value = 1281.12M
                        }
                    }
                }
            };

            var buff = ReportManager.BuildRecordSummaryReport(data);

            //using (var fs = File.Open(@"D:\aaa.pdf", FileMode.OpenOrCreate, FileAccess.Write))
            //{
            //    fs.Write(buff,0,buff.Length);
            //}

            Assert.IsNotNull(buff);
        }

        [TestMethod]
        public void AppendixReportTest()
        {
            var data = new RecordAppendixInfo
            {
                RecordDate = DateTime.Now,
                AppendixNumber = 1,
                SubSets = new List<RecordAppendixSubSet>
                {
                    new RecordAppendixSubSet
                    {
                        Title = "No hejka! :)",
                        Rows = new ObservableCollection<RecordAppendixReportRowInfo>
                        {
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Cena uzysku",
                                Value = 200
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Cena uzysku",
                                Value = 200
                            }
                        }
                    },
                    new RecordAppendixSubSet
                    {
                        Title = "No hejka 2! :)",
                        Rows = new ObservableCollection<RecordAppendixReportRowInfo>
                        {
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Cena uzysku",
                                Value = 200
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Cena uzysku",
                                Value = 200
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            }
                        }
                    },
                    new RecordAppendixSubSet
                    {
                        Title = "No hejka 3! :)",
                        Rows = new ObservableCollection<RecordAppendixReportRowInfo>
                        {
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Cena uzysku",
                                Value = 200
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Cena uzysku",
                                Value = 200
                            },
                            new RecordAppendixReportRowInfo
                            {
                                Name = "Ubytek",
                                Value = 150
                            }
                        }
                    }
                }
            };

            var buff = ReportManager.BuildRecordAnnexReport(data);

            Assert.IsNotNull(buff);
        }
    }
}