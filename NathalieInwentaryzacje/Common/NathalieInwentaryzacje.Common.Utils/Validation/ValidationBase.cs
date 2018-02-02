using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using NathalieInwentaryzacje.Common.Utils.Interfaces;
using NathalieInwentaryzacje.Common.Utils.Validation.Attributes;

namespace NathalieInwentaryzacje.Common.Utils.Validation
{
    public abstract class ValidationBase : NotifyBase, IDataErrorInfo, IValidationBase
    {
        #region Private fields

        private bool _isChildSubscribed;
        private bool _isSubscribed;
        private Dictionary<string, PropertyInfo> _propertyGetters;
        private Dictionary<string, List<ValidationAttribute>> _validators;

        /// <summary>
        ///     Metoda dla własnej walidacji dodatkowej
        ///     Object jest wartością propertiesa
        ///     string jest nazwą propertiesa
        ///     List -string  jest listą errorów - jeżeli brak pusta lista
        /// </summary>
        public Func<object, string, List<string>> CustomValidationMethod;

        private readonly string _childPropertyName;
        private readonly List<ValidationBase> _childsProperties = new List<ValidationBase>();
        private readonly List<ValidationAttribute> _parentValidation;
        private readonly Dictionary<string, string> _propertyErrors = new Dictionary<string, string>();

        #endregion

        #region ctor

        /// <summary>
        ///     Validations the view model base.
        /// </summary>
        protected ValidationBase()
        {
            LoadValidatorsAndPropertiesGetters();
        }

        /// <summary>
        ///     Validations the view model base.
        /// </summary>
        protected ValidationBase(List<ValidationAttribute> parentValidation, string childPropertyName)
        {
            _parentValidation = parentValidation;
            _childPropertyName = childPropertyName;
            LoadValidatorsAndPropertiesGetters();
        }
        #endregion

        public string this[string propertyName]
        {
            get
            {
                try
                {
                    SubscribeForCollectionChanges();

                    LoadValidatorsAndPropertiesGetters();

                    string propertyErrors;
                    if (_propertyGetters.ContainsKey(propertyName))
                    {
                        var propertyValue = _propertyGetters[propertyName].GetValue(this, null);
                        var errorMessags = new List<string>();
                        //jeżeli istnieją atrybuty walidacyjne to sprawdza je
                        if (_validators.ContainsKey(propertyName))
                        {
                            errorMessags.AddRange(_validators[propertyName]
                                .Where(
                                    v =>
                                        v.GetValidationResult(propertyValue, new ValidationContext(this)) !=
                                        ValidationResult.Success)
                                .Select(v => v.ErrorMessage).ToArray());
                        }
                        //jeżeli jest dodatkowa walidacja to ją dodaje
                        if (CustomValidationMethod != null)
                            errorMessags.AddRange(CustomValidationMethod(propertyValue, propertyName));

                        propertyErrors = errorMessags.Count > 0
                            ? string.Join(Environment.NewLine, errorMessags)
                            : string.Empty;
                    }
                    else
                    {
                        propertyErrors = string.Empty;
                    }

                    //dodanie błędów propertiesa do listy globalnych błędów  
                    if (!_propertyErrors.ContainsKey(propertyName))
                        _propertyErrors.Add(propertyName, propertyErrors);
                    else
                    {
                        _propertyErrors[propertyName] = propertyErrors;
                    }


                    return propertyErrors;
                }
                finally
                {
                    OnPropertyChanged(() => Error);
                    OnPropertyChanged(() => IsValid);
                }
            }
        }

        public string Error
        {
            get
            {
                SubscribeForChildChanges();
                // zmieniono na przechowywanie info o errors dla optymalizacji

                var errors = _propertyErrors.Where(p => !string.IsNullOrEmpty(p.Value)).Select(p => p.Value).ToList();

                //zebranie błędów z childów
                foreach (var childsProperty in _childsProperties)
                {
                    if (!childsProperty.IsValid)
                        errors.AddRange(
                            childsProperty._propertyErrors.Where(p => !string.IsNullOrEmpty(p.Value))
                                .Select(p => p.Value)
                                .ToList());
                }

                return string.Join(Environment.NewLine, errors.Distinct());
            }
        }

        public bool IsValid => string.IsNullOrEmpty(Error);

        protected void SetErrorMessage(string propertyName, string errorMessage)
        {
            var errors = _propertyErrors.Where(p => !string.IsNullOrEmpty(p.Value)).Select(p => p.Value).ToList();
            //errors.Add(errorMessage);
            if (string.IsNullOrEmpty(errorMessage))
            {
                _propertyErrors[propertyName] = null;
            }
            else
            {
                errors.Add(errorMessage);
                _propertyErrors[propertyName] = string.Join(Environment.NewLine, errors);
            }

            NotifyPropertyChanged(() => Error);
            NotifyPropertyChanged(() => IsValid);
        }

        public bool IsPropertyValid([CallerMemberName] string propertyName = null)
        {
            var propertyValue = _propertyGetters[propertyName ?? throw new ArgumentNullException(nameof(propertyName))].GetValue(this, null);
            return
                _validators[propertyName].All(
                    v => v.GetValidationResult(propertyValue, new ValidationContext(this)) != ValidationResult.Success);
        }

        /// <summary>
        /// Pobiera ErrorMsg dla wybranej 
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="selectorExpression"></param>
        /// <returns></returns>
        public string GetErrorMsg<TE>(Expression<Func<TE>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException(nameof(selectorExpression));
            if (!(selectorExpression.Body is MemberExpression body))
                throw new ArgumentException("The body must be a member expression");

            var propertyValue = _propertyGetters[body.Member.Name].GetValue(this, null);

            var sb = new StringBuilder();

            foreach (var validator in _validators[body.Member.Name])
            {
                if (validator.GetValidationResult(propertyValue, new ValidationContext(this)) != ValidationResult.Success)
                {
                    sb.Append(validator.ErrorMessage);
                }
            }
            return sb.ToString();
        }

        public bool IsPropertyValid<TE>(Expression<Func<TE>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException(nameof(selectorExpression));
            if (!(selectorExpression.Body is MemberExpression body))
                throw new ArgumentException("The body must be a member expression");

            var propertyValue = _propertyGetters[body.Member.Name].GetValue(this, null);
            return
                _validators[body.Member.Name].All(
                    v => v.GetValidationResult(propertyValue, new ValidationContext(this)) == ValidationResult.Success);
        }

        public List<ValidationAttribute> GetValidationAttributes(string name)
        {
            LoadValidatorsAndPropertiesGetters();

            if (_validators.ContainsKey(name))
                return _validators[name];

            return null;
        }

//        string IValidationBase.Error
//        {
//            get { return _error; }
//        }

        #region Helper methods

        //tylko podczas uzycia jak kolecja zainstancjonowana
        private void SubscribeForCollectionChanges()
        {
            if (!_isSubscribed)
            {
                //Zapisanie się na zmiany IsValid lub is Error w kolecji
                var colVali = GetType().GetProperties().Where(p => GetCollectionValidations(p).Length != 0);
                //Dla kazdego parametru z validacja listy
                foreach (var info in colVali)
                {
                    if (info.GetValue(this, new object[] { }) is INotifyPropertyChanged item)
                        item.PropertyChanged += item_PropertyChanged;
                }
                _isSubscribed = true;
            }
        }

        private List<ValidationAttribute> GetValidations(PropertyInfo property)
        {
            return
                new List<ValidationAttribute>(
                    (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true));
        }

        private ValidateCollectionAttribute[] GetCollectionValidations(PropertyInfo property)
        {
            return
                (ValidateCollectionAttribute[])property.GetCustomAttributes(typeof(ValidateCollectionAttribute), true);
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(() => Error);
            OnPropertyChanged(() => IsValid);
            NotifyPropertyChanged(e.PropertyName);
        }

        private void GetPropertyGetters()
        {
            var getterList = GetType()
                .GetProperties().ToList();

            _propertyGetters = getterList.ToDictionary(p => p.Name);
        }

        private void SubscribeForChildChanges()
        {
            if (!_isChildSubscribed)
            {
                //zapisanie się na zmiany childów
                foreach (var propertyInfo in _propertyGetters.Values)
                {
                    if ((propertyInfo.PropertyType.IsGenericType &&
                        propertyInfo.PropertyType.BaseType == typeof(ValidationBase)) ||
                        (propertyInfo.PropertyType.BaseType != null &&
                         propertyInfo.PropertyType.BaseType.BaseType != null &&
                         propertyInfo.PropertyType.BaseType.BaseType == typeof(ValidationBase)))
                    {
                        if (propertyInfo.GetValue(this, null) is ValidationBase property)
                        {
                            property.PropertyChanged += item_PropertyChanged;
                            _childsProperties.Add(property);
                        }
                    }
                }
                _isChildSubscribed = true;
            }
        }

        private void LoadValidatorsAndPropertiesGetters()
        {
            if (_validators == null || _propertyGetters == null)
            {
                _validators = GetType()
                    .GetProperties()
                    .Where(p => GetValidations(p).Count != 0)
                    .ToDictionary(p => p.Name, GetValidations);

                GetPropertyGetters();

                //jezeli byl parent validator dla childow to dodajemy
                if (_parentValidation != null && !string.IsNullOrEmpty(_childPropertyName))
                {
                    //dodanie / ustawienie validatorów
                    if (_validators.ContainsKey(_childPropertyName))
                        _validators[_childPropertyName].AddRange(_parentValidation);
                    else
                    {
                        _validators.Add(_childPropertyName, _parentValidation.ToList());
                    }

                    //dodanie / ustawienie gettera dla _childproperty jeżeli nie było
                    if (!_propertyGetters.ContainsKey(_childPropertyName))
                    {
                        var prop = GetType().GetProperties().SingleOrDefault(p => p.Name == _childPropertyName);
                        _propertyGetters.Add(_childPropertyName, prop);
                    }
                }
            }
        }

        #endregion
    }
}
