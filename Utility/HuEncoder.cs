using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class HuEncoder
    {
        #region 定数
        /// <summary>
        /// Shift-JISエンコーディング
        /// </summary>
        private static readonly Encoding SJIS = Encoding.GetEncoding("shift_jis");

        /// <summary>
        /// デフォルトエンコーディング
        /// </summary>
        public static readonly Encoding DefaultEncoding = SJIS;
        #endregion

        #region 列挙体
        /// <summary>
        /// 変換方式
        /// </summary>
        public enum EncodeType
        {
            /// <summary>
            /// SHA1ハッシュ作成
            /// </summary>
            SHA1,

            /// <summary>
            /// MD5ハッシュ作成
            /// </summary>
            MD5,
        }
        #endregion
    }
}
