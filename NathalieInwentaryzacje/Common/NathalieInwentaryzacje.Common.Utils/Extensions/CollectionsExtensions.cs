using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NathalieInwentaryzacje.Common.Utils.Extensions
{
    public static class CollectionsExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
