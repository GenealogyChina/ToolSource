using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Enum.Utility
{
    public enum HeMessageType
    {
        /// <summary>
        /// 情報
        /// </summary>
        Information = 0,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// エラー
        /// </summary>
        Error,

        /// <summary>
        /// 質問
        /// </summary>
        Question,

        /// <summary>
        /// デバッグ
        /// </summary>
        Debug,
    }
}
