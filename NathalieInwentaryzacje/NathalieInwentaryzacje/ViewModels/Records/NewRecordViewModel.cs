using System.Linq;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class NewRecordViewModel : DetailsScreen<NewRecordInfo>
    {
        private readonly ITemplatesManager _templatesManager = IoC.Get<ITemplatesManager>();
        private readonly IRecordsManager _recordsManager = IoC.Get<IRecordsManager>();

        public NewRecordViewModel()
        {
            Context = new NewRecordInfo();
            LoadTypes();
        }

        public void Cancel()
        {
            TryClose();
        }

        public async void Create()
        {
            if (!Context.IsRecordTypeSelected)
            {
                await ShowMessage("Formularz niekompletny",
                    "Nie wskazano szablonów inwentaryzacji!");
                return;
            }

            _recordsManager
                .CreateRecord(Context);
            TryClose(true);
        }

        private void LoadTypes()
        {
            Context.RecordTypes = _templatesManager.GetTemplates().Where(x=>x.IsEnabled).Select(x=>new NewRecordTypeInfo
            {
                TemplateInfo= x,
                IsSelected = true
            }).ToList();
        }
    }
}
