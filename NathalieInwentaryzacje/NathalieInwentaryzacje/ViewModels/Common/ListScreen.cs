using System.Collections.Generic;
using NathalieInwentaryzacje.Main.Interfaces;

namespace NathalieInwentaryzacje.ViewModels.Common
{
    public class ListScreen<T> : ScreenBase, ILoadable
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public IEnumerable<T> Context
        {
            get => _context;
            set
            {
                if (Equals(value, _context)) return;
                _context = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// The _selected context item
        /// </summary>
        private T _selectedContextItem;

        private IEnumerable<T> _context;

        public T SelectedContextItem
        {
            get => _selectedContextItem;
            set
            {
                _selectedContextItem = value;
                NotifyOfPropertyChange(() => SelectedContextItem);
            }
        }

        public virtual void LoadData()
        {

        }

        public virtual void RefreshList()
        {
            LoadData();
        }
    }
}
