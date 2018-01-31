using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
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

        public TemplateViewModel(TemplateInfo templateInfo)
        {
            Context = templateInfo ?? new TemplateInfo();
        }

        public void Save()
        {
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
