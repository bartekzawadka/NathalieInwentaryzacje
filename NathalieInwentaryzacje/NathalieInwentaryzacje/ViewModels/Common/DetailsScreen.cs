namespace NathalieInwentaryzacje.ViewModels.Common
{
    public class DetailsScreen<T> : ScreenBase
    {
        private T _context;

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public T Context
        {
            get => _context;
            set
            {
                if (Equals(value, _context)) return;
                _context = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
