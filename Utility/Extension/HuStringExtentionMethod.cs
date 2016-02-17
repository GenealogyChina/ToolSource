using System;
using System.Text;
using Utility.Enum;

namespace Utility.Extension
{
    public static class HuStringExtentionMethod
    {
        #region 静的なパブリック メソッド
        /// <summary>
        /// 指定した列挙体に指定された文字列の値があるかどうか
        /// </summary>
        /// <typeparam name="T">列挙体の型</typeparam>
        /// <param name="value">文字列</param>
        /// <returns>指定した列挙体である場合True</returns>
        public static bool IsEnum<T>(this string value) where T : struct
        {
            return value != null && System.Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// 指定した列挙体に指定された文字列の値があるかどうか
        /// </summary>
        /// <typeparam name="T">列挙体の型</typeparam>
        /// <param name="value">文字列</param>
        /// <returns>指定した列挙体である場合True</returns>
        public static bool IsEnumClass<T>(this string value) where T : HecEnum
        {
            return value != null && HecEnum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// 文字列を指定する列挙体に変換する
        /// </summary>
        /// <typeparam name="T">指定する列挙体</typeparam>
        /// <param name="value">文字列</param>
        /// <returns>列挙体</returns>
        public static T ToEnum<T>(this string value) where T : struct
        {
            var result = default(T);
            if (!System.Enum.TryParse<T>(value, out result))
            {
                throw new ArgumentException(HuMessages.GetMessageText(HecFwMessageId.MF0050CC, typeof(T).Name));
            }
            return result;
        }

        /// <summary>
        /// 文字列を指定する列挙体に変換する
        /// </summary>
        /// <typeparam name="T">指定する列挙体</typeparam>
        /// <param name="value">文字列</param>
        /// <returns>列挙体</returns>
        public static T ToEnumClass<T>(this string value) where T : HecEnum
        {
            var result = default(T);
            if (!HecEnum.TryParse<T>(value, out result))
            {
                throw new ArgumentException(HuMessages.GetMessageText(HecFwMessageId.MF0050CC, typeof(T).Name));
            }
            return result;
        }

        public static string ToTitleCase(this string value)
        {
            string[] values = value.Split('_');
            StringBuilder sb = new StringBuilder();
            foreach (string item in values)
            {
                if (sb.Length  == 0 && item.Substring(0, 1).ToLower().Equals("v"))
                {
                    sb.Append(item.Substring(0, 2).ToUpper() + item.Substring(2, item.Length - 2));
                }
                else
                {
                    sb.Append(item.Substring(0, 1).ToUpper() + item.Substring(1, item.Length - 1));
                }
            }
            return sb.ToString();
        }

        public static string ToPrivateDefinition(this string value)
        {
            string[] values = value.Split('_');
            StringBuilder sb = new StringBuilder();
            foreach (string item in values)
            {
                if (sb.Length == 0)
                {
                    sb.Append(item);
                }
                else
                {
                    sb.Append(item.Substring(0, 1).ToUpper() + item.Substring(1, item.Length - 1));
                }
                
            }
            return sb.ToString();
        }

        public static string ToPublicDefinition(this string value)
        {
            string[] values = value.Split('_');
            StringBuilder sb = new StringBuilder();
            foreach (string item in values)
            {
                sb.Append(item.Substring(0, 1).ToUpper() + item.Substring(1, item.Length - 1));
            }
            return sb.ToString();
        }
        #endregion
    }
}
