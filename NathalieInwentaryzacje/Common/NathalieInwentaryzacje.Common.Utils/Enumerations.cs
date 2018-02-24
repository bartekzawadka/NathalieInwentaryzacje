using System;
using System.ComponentModel;

namespace NathalieInwentaryzacje.Common.Utils
{
    public static class Enumerations
    {
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
