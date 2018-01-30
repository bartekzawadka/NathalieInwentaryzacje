using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Enums;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class NewRecordViewModel : ScreenBase
    {
        private List<NewRecordTypeInfo> _entries;

        public List<NewRecordTypeInfo> Entries
        {
            get => _entries;
            set
            {
                if (Equals(value, _entries)) return;
                _entries = value;
                NotifyOfPropertyChange();
            }
        }

        public NewRecordViewModel()
        {
            LoadTypes();
        }

        private void LoadTypes()
        {
            var types = Enum.GetValues(typeof(RecordType));

            var entries = new List<NewRecordTypeInfo>();

            foreach (var type in types)
            {
                entries.Add(new NewRecordTypeInfo
                {
                    RecordType = (RecordType)type
                });
            }

            Entries = entries;
        }
    }
}
