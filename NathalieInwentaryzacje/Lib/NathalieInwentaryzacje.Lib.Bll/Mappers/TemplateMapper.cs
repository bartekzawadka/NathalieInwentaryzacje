using System.Collections.Generic;
using System.Linq;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Appendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Bll.Mappers
{
    public class TemplateMapper
    {
        public static Template ToTemplate(TemplateInfo value)
        {
            return new Template
            {
                Id = value.Id,
                Name = value.Name,
                IsEnabled = value.IsEnabled,
                TemplateFileName = value.TemplateFilePath
            };
        }

        public static TemplateInfo ToTemplateInfo(Template value)
        {
            return new TemplateInfo
            {
                Id = value.Id,
                Name = value.Name,
                IsEnabled = value.IsEnabled,
                TemplateFilePath = value.TemplateFileName
            };
        }

        public static Appendix AppendixInfoToAppendixFile(RecordAppendixInfo value)
        {
            if (value == null)
                return null;

            var appendix = new Appendix
            {
                AppendixNumber = value.AppendixNumber
            };

            if (value.SubSets == null || value.SubSets.Count <= 0)
                return appendix;

            var dataSets = new List<AppendixDataSet>();
            foreach (var recordAppendixSubSet in value.SubSets)
            {
                var dataSet = new AppendixDataSet
                {
                    Title = recordAppendixSubSet.Title,
                    Entries = recordAppendixSubSet.Rows.Select(x=>new AppendixDataSetEntry()
                    {
                        Name = x.Name,
                        Value = x.Value
                    }).ToArray()
                };

                dataSets.Add(dataSet);
            }

            appendix.DataSets = dataSets.ToArray();

            return appendix;
        }

        public static RecordAppendixInfo AppendixFileToAppendixInfo(Appendix appendix)
        {
            if (appendix == null)
                return null;

            var appendixInfo = new RecordAppendixInfo
            {
                AppendixNumber = appendix.AppendixNumber
            };

            appendixInfo.SubSets = appendix.DataSets?.Select(x => new RecordAppendixSubSet
            {
                Title = x.Title,
                Rows = x.Entries?.Select(y => new RecordAppendixReportRowInfo
                {
                    Name = y.Name,
                    Value = y.Value
                }).ToObservableCollection()
            }).ToList();

            return appendixInfo;
        }
    }
}
