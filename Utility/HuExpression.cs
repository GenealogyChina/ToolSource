using System;
using System.Linq.Expressions;

namespace Utility
{
    public static class HuExpression
    {
        #region 定数
        /// <summary>
        /// プロパティ識別子
        /// </summary>
        private const string ID_PROPERTY = "Property";
        #endregion

        #region 静的なパブリック メソッド
        /// <summary>
        /// 指定されたラムダ式のメンバー名を返す
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="e">ラムダ式</param>
        /// <returns>メンバー名</returns>
        public static string GetName<T>(Expression<Func<T>> e)
        {
            return ((MemberExpression)e.Body).Member.Name;
        }

        /// <summary>
        /// 指定されたラムダ式のメンバー名(Property除く)を返す
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="e">ラムダ式</param>
        /// <returns>メンバー名</returns>
        public static string GetPropName<T>(Expression<Func<T>> e)
        {
            var name = ((MemberExpression)e.Body).Member.Name;
            var idx = name.LastIndexOf(ID_PROPERTY);
            return idx != HuNumber.ErrIndex ? name.Substring(HuNumber.StartIndex, idx) : name;
        }
        #endregion
    }
}
