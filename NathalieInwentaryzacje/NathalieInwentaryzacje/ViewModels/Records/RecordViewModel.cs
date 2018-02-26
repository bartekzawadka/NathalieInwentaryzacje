using System.Linq;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class RecordViewModel : DetailsScreen<RecordInfo>
    {
        private readonly ITemplatesManager _templatesManager = IoC.Get<ITemplatesManager>();
        private readonly IRecordsManager _recordsManager = IoC.Get<IRecordsManager>();

        private readonly RecordListInfo _recordItem;
        private readonly bool _isAppendMode;

        public RecordViewModel()
        {
            Context = new RecordInfo();
            _isAppendMode = false;
            LoadTypes();
        }

        public RecordViewModel(RecordListInfo recordItem)
        {
            _recordItem = recordItem;

            Context = new RecordInfo
            {
                RecordsDate = recordItem.RecordDate,
                DisplayTitle = "Dodaj pozycje",
                Id = recordItem.RecordId
            };
            _isAppendMode = true;
            LoadTypes();
        }

        public void Cancel()
        {
            TryClose();
        }

        public async void Save()
        {
            if (!Context.IsRecordTypeSelected)
            {
                await ShowMessage("Formularz niekompletny",
                    "Nie wskazano szablonów inwentaryzacji!");
                return;
            }

            _recordsManager
                .CreateOrUpdateRecord(Context);
            TryClose(true);
        }

        private void LoadTypes()
        {
            Context.RecordTypes = _templatesManager.GetTemplates().Where(x => x.IsEnabled).Select(x => new RecordTypeInfo
            {
                TemplateInfo = x,
                IsSelected = true,
                IsEnabled = true
            }).ToList();

            if (_isAppendMode)
            {
                var templateIds = _templatesManager.GetTemplates().Select(x => x.Id);

                var existingTypes = _recordItem.RecordsInfo.Where(x => templateIds.Contains(x.TemplateId))
                    .Select(x => x.TemplateId).ToList();

                foreach (var contextRecordType in Context.RecordTypes)
                {
                    if (existingTypes.Contains(contextRecordType.TemplateInfo.Id))
                    {
                        contextRecordType.IsEnabled = false;
                    }
                    else
                    {
                        contextRecordType.IsSelected = false;
                    }
                }
            }
        }
    }
}
