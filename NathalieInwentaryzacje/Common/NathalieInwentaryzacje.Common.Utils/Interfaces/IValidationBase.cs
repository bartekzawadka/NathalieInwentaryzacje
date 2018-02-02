using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace NathalieInwentaryzacje.Common.Utils.Interfaces
{
    public interface IValidationBase
    {
        string Error { get; }
        bool IsValid { get; }
        bool IsPropertyValid([CallerMemberName] string propertyName = null);
        bool IsPropertyValid<TE>(Expression<Func<TE>> selectorExpression);
    }
}
