using System.Collections.Generic;
using System.Data;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Records;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Bll.Mappers
{
    public class RecordMapper
    {
        public static RecordEntryInfo ToRecordEntryInfo(Record record)
        {
            var recordInfo =  new RecordEntryInfo
            {
                TemplateId = record.TemplateId,
                RecordDate = record.RecordDate,
                RecordName = record.Name,
                RecordDisplayName = record.RecordTitle,
                RecordId = record.RecordId,
                SumUpColumnName = record.SumUpColumnName
            };

            var dataSet = new DataTable();

            dataSet.AcceptChanges();

            if (record.Entries != null)
            {
                foreach (var recordEntry in record.Entries)
                {
                    var row = dataSet.NewRow();
                    foreach (var recordEntryColumn in recordEntry.Columns)
                    {
                        row[recordEntryColumn.ColumnName] = recordEntryColumn.ColumnValue;
                    }

                    dataSet.Rows.Add(row);
                    row.AcceptChanges();
                    dataSet.AcceptChanges();
                }
            }

            recordInfo.RecordEntryTable = dataSet;

            return recordInfo;
        }

        public static Record ToRecord(RecordEntryInfo info)
        {
            var record = new Record
            {
                Name = info.RecordName,
                RecordDate = info.RecordDate,
                RecordId = info.RecordId,
                RecordTitle = info.RecordDisplayName,
                TemplateId = info.TemplateId,
                SumUpColumnName = info.SumUpColumnName
            };

            if (info.RecordEntryTable != null)
            {
                var entries = new List<RecordEntry>();

                foreach (DataRow dataRow in info.RecordEntryTable.Rows)
                {
                    var columns = new List<RecordEntryColumn>();
                    foreach (DataColumn dataColumn in info.RecordEntryTable.Columns)
                    {
                        columns.Add(new RecordEntryColumn
                        {
                            ColumnName = dataColumn.ColumnName,
                            ColumnValue = dataRow[dataColumn.ColumnName]?.ToString(),
                            IsReadOnly = string.Equals(dataColumn.ColumnName, info.SumUpColumnName)
                        });
                    }

                    entries.Add(new RecordEntry
                    {
                        Columns = columns.ToArray()
                    });
                }

                record.Entries = entries.ToArray();
            }

            return record;
        }
    }
}
