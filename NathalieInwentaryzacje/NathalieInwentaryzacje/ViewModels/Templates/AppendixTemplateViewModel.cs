using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Templates
{
    public class AppendixTemplateViewModel : DetailsScreen<RecordAppendixInfo>
    {
        private readonly ITemplatesManager _templatesManager = IoC.Get<ITemplatesManager>();
        private ObservableCollection<RecordAppendixSubSet> _subSets;

        public ObservableCollection<RecordAppendixSubSet> SubSets
        {
            get => _subSets;
            set
            {
                if (Equals(value, _subSets)) return;
                _subSets = value;
                NotifyOfPropertyChange();
            }
        }

        public AppendixTemplateViewModel()
        {
            Context = _templatesManager.GetRecordAppendixTemplate();
            SubSets = Context.SubSets == null
                ? new ObservableCollection<RecordAppendixSubSet>()
                : new ObservableCollection<RecordAppendixSubSet>(Context.SubSets);
            Initialize();
        }

        private void Initialize()
        {
            DisplayName = "Załącznik nr " + Context.AppendixNumber + " do inwentaryzacji";
        }

        public void AddCostSection()
        {
            SubSets.Add(new RecordAppendixSubSet
            {
                Rows = new ObservableCollection<RecordAppendixReportRowInfo>()
            });
        }

        public void DeleteSubSet(RecordAppendixSubSet context)
        {
            SubSets.Remove(context);
        }

        public async void Save()
        {
            if (SubSets.Any(x => string.IsNullOrEmpty(x.Title)))
            {
                await ShowMessage("Brak tytułu", "Przynajmniej jeden wykaz kosztów nie zawiera tytułu");
                return;
            }

            Context.SubSets = SubSets.ToList();
            _templatesManager.SaveRecordAppendixTemplate(Context);
            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
