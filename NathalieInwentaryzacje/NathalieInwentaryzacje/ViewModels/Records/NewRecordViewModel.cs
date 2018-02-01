using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Enums;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class NewRecordViewModel : ScreenBase
    {
        private List<NewRecordTypeInfo> _recordTypes;
        private DateTime _recordDate;

        public List<NewRecordTypeInfo> RecordTypes
        {
            get => _recordTypes;
            set
            {
                if (Equals(value, _recordTypes)) return;
                _recordTypes = value;
                NotifyOfPropertyChange();
            }
        }

        public DateTime RecordDate
        {
            get => _recordDate;
            set
            {
                if (value.Equals(_recordDate)) return;
                _recordDate = value;
                NotifyOfPropertyChange();
            }
        }


        public NewRecordViewModel()
        {
            LoadTypes();
        }

        public void Cancel()
        {
            TryClose();
        }

        public void Create()
        {
            IoC.Get<IRecordsManager>()
                .CreateRecord(RecordDate, RecordTypes.Where(x => x.IsSelected).Select(x => x.TemplateInfo));
            TryClose(true);
        }

        private void LoadTypes()
        {
            RecordTypes = IoC.Get<ITemplatesManager>().GetTemplates().Where(x=>x.IsEnabled).Select(x=>new NewRecordTypeInfo
            {
                TemplateInfo= x,
                IsSelected = false
            }).ToList();
//            var types = Enum.GetValues(typeof(RecordType));
//
//            var entries = new List<NewRecordTypeInfo>();
//
//            foreach (var type in types)
//            {
//                entries.Add(new NewRecordTypeInfo
//                {
//                    RecordType = (RecordType)type
//                });
//            }
//
//            RecordTypes = entries;
        }
    }
}
