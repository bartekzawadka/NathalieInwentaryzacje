using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Templates
{
    public class TemplatesListViewModel : ListScreen<TemplateInfo>
    {
        private readonly ITemplatesManager _templatesManager = IoC.Get<ITemplatesManager>();

        public TemplatesListViewModel()
        {
            LoadData();
        }

        public override void LoadData()
        {
            Context = _templatesManager.GetTemplates(true);
        }

        public void NewTemplate()
        {
            if (ShowDialog(new TemplateViewModel()) == true)
            {
                LoadData();
            }
        }

        public override async void SelectedContextItemDoubleClick(object context)
        {
            var tInfo = context as TemplateInfo;

            if (string.IsNullOrEmpty(tInfo?.Id))
            {
                await ShowMessage("Błąd", "Wskazany szablon jest nieprawidłowy (brak identyfikatora)");
                return;
            }

            if (ShowDialog(new TemplateViewModel(_templatesManager.GetTemplate(tInfo.Id))) == true)
            {
                LoadData();
            }
        }
    }
}
