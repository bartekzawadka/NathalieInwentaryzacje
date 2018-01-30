using System;
using System.Collections.Generic;
using System.Text;
using NathalieInwentaryzacje.Common.Utils;
using NathalieInwentaryzacje.Lib.Contracts.Enums;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class NewRecordTypeInfo
    {
        public RecordType RecordType { get; set; }

        public string RecordTypeName => Enumerations.GetEnumDescription(RecordType);

        public bool IsSelected { get; set; }
    }
}
