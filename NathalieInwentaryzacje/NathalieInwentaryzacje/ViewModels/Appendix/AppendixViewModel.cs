using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Appendix
{
    public class AppendixViewModel : DetailsScreen<RecordAppendixInfo>
    {
        private readonly IAppendixManager _appendixManager = IoC.Get<IAppendixManager>();
        private ObservableCollection<RecordAppendixSubSet> _subSets;

        public ObservableCollection<RecordAppendixSubSet> SubSets
        {
            get => _subSets;
            set
            {
                if (Equals(value, _subSets)) return;

                foreach (var recordAppendixSubSet in value)
                {
                    recordAppendixSubSet.Rows = recordAppendixSubSet.Rows.Where(x => !string.IsNullOrEmpty(x.Name))
                        .ToObservableCollection();
                }

                _subSets = value;
                NotifyOfPropertyChange();
            }
        }

        public AppendixViewModel(DateTime? recordDate)
        {
            Context = _appendixManager.GetAppendix(recordDate);
            if (recordDate != null && recordDate != new DateTime())
                Context.RecordDate = recordDate.Value;
            Initialize();
        }

        private void Initialize()
        {
            SubSets = Context.SubSets == null
                ? new ObservableCollection<RecordAppendixSubSet>()
                : new ObservableCollection<RecordAppendixSubSet>(Context.SubSets);
            var displayName = Context.RecordDate != new DateTime()
                ? "Załącznik nr " + Context.AppendixNumber + " do inwentaryzacji (" +
                  Context.RecordDate.ToRecordDateString() + ")"
                : "Szablon załącznika do inwentaryzacji";
            DisplayName = displayName;
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
            _appendixManager.SaveAppendix(Context);
            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
