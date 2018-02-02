using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Bll.Mappers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Templates
{
    public class TemplateViewModel : DetailsScreen<TemplateInfo>
    {
        public ObservableCollection<TemplateColumn> TemplateColumns
        {
            get
            {
                if(Context?.Columns == null)
                    return new ObservableCollection<TemplateColumn>();
                return new ObservableCollection<TemplateColumn>(Context.Columns);
            }
            set
            {
                if (Context != null && value != null)
                {
                    Context.Columns = value.ToArray();
                    NotifyOfPropertyChange();
                }
            }
        }

        public TemplateViewModel() : this(null)
        {

        }

        public TemplateViewModel(TemplateInfo template)
        {
            Context = template ?? new TemplateInfo();
        }

        public void UpdateColumns(DataGridCellEditEndingEventArgs args)
        {
            var value = (args.EditingElement as TextBox)?.Text;
            if (string.IsNullOrEmpty(value))
                return;

            var templateColumns = TemplateColumns;

            if (args.Row.IsNewItem)
            {
                templateColumns.Add(new TemplateColumn
                {
                    Name = value
                });
            }
            else
            {
                var index = args.Row.GetIndex();
                templateColumns[index].Name = value;
            }

            TemplateColumns = templateColumns.Where(x => !string.IsNullOrEmpty(x.Name)).ToObservableCollection();
        }

        public async void Save()
        {
            if (TemplateColumns.Count == 0)
            {
                await ShowMessage("Brak kolumn", "Nie określono kolumn szablonu");
                return;
            }

            IoC.Get<ITemplatesManager>().CreateOrUpdateTemplate(Context);
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
