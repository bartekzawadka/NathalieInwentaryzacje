using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace NathalieInwentaryzacje.Common.Utils
{
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            OnPropertyChanged(selectorExpression);
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <typeparam name="TE">The type of the E.</typeparam>
        /// <param name="selectorExpression">The selector expression.</param>
        protected virtual void OnPropertyChanged<TE>(Expression<Func<TE>> selectorExpression)
        {
            NotifyPropertyChanged(ObjectHelper.NameOf(selectorExpression));
        }
    }
}
