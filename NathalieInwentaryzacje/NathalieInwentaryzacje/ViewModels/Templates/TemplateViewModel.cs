using System.IO;
using Caliburn.Micro;
using Microsoft.Win32;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Templates
{
    public sealed class TemplateViewModel : DetailsScreen<TemplateInfo>
    {
        public TemplateViewModel() : this(null)
        {

        }

        public TemplateViewModel(TemplateInfo template)
        {
            Context = template ?? new TemplateInfo();
        }

        public void ChooseTemplate()
        {
            var fDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Microsoft Excel (*.xlsx)|*.xlsx|Microsoft Excel (*.xls)|*.xls",
                FilterIndex = 1
            };
            if (fDialog.ShowDialog() == true)
            {
                Context.TemplateFilePath = fDialog.FileName;
                NotifyOfPropertyChange(nameof(Context));
            }
        }


        public void Save()
        {
            IoC.Get<ITemplatesManager>().CreateOrUpdateTemplate(Context);
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
