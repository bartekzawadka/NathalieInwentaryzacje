using System.Collections.Generic;

namespace NathalieInwentaryzacje.ViewModels.Common
{
    public class ListScreen<T> : ScreenBase
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public IEnumerable<T> Context { get; set; }

        /// <summary>
        /// The _selected context item
        /// </summary>
        private T _selectedContextItem;

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
    }
}
