using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class HuXmlMessageInfo : HuMessageInfo
    {
        #region 列挙体
        /// <summary>
        /// スキップ情報
        /// </summary>
        public enum SkipInfo
        {
            /// <summary>
            /// スキップしない
            /// </summary>
            NoSkip,

            /// <summary>
            /// スキップする
            /// </summary>
            Skip,

            /// <summary>
            /// ユーザー側で判断する
            /// </summary>
            User
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// メッセージ表示をスキップするかどうか
        /// </summary>
        /// <value> スキップ情報</value>
        public SkipInfo Skip { get; set; }

        /// <summary>
        /// Xmlで定義されたメッセージ内容
        /// </summary>
        public string XmlMessage { get; set; }
        #endregion
    }
}
