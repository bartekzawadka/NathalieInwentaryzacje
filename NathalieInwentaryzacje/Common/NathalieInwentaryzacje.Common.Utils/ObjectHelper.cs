using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NathalieInwentaryzacje.Common.Utils
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Pobiera nazwę członka (member) klasy.
        /// Przykład:
        /// ObjectHelper.NameOf(()=>testClass1.Length)  //wartość zwaracana: "Length";
        /// </summary>
        /// <typeparam name="T">Typ obiektu z którego ma być pobrna nazwa członka (member). Może być wywołany niejawnie</typeparam>
        /// <param name="e">Wyrażenie </param>
        /// <returns>nazwa członka (member) klasy.</returns>
        public static string NameOf<T>(Expression<Func<T>> e)
        {

            return MemberInfo(e).Name;

        }

        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static MemberInfo MemberInfo<T>(Expression<Func<T>> e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            var body = (MemberExpression)e.Body;
            if (body == null)
                throw new ArgumentException("'expression' should be a member expression");

            return body.Member;
        }

        /// <summary>
        /// Sprawdza czy dany obiekt jest nullem albo pusty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || String.IsNullOrWhiteSpace(obj.ToString());
        }
    }
}
