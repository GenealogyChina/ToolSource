using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;
using Utility.Enum.Utility;

namespace Utility
{
    public static class HuMessages
    {
        #region 定数
        /// <summary>
        /// FWメッセージ用XMLファイル
        /// </summary>
        private const string FW_MESSAGE_XML = "CodeMZ.CMZ.Fw.Utility.Resources.messageFw.xml";

        /// <summary>
        /// XMLノードの名前(MSG)
        /// </summary>
        private const string XML_NODE_MSG = "/msgs/msg";

        /// <summary>
        /// XMLの属性(Id)
        /// </summary>
        private const string XML_ATTR_ID = "id";

        /// <summary>
        /// XMLの属性(Type)
        /// </summary>
        private const string XML_ATTR_TYPE = "type";

        /// <summary>
        /// XMLの属性(Skip)
        /// </summary>
        private const string XML_ATTR_SKIP = "skip";

        /// <summary>
        /// 改行用シンボル
        /// </summary>
        private const string RETURN_SYMBOL = "\\r\\n";

        /// <summary>
        /// メッセージ種別のマップ
        /// </summary>
        private static Dictionary<string, HeMessageType> msgTypes = new Dictionary<string, HeMessageType>()
        {
            { "E", HeMessageType.Error },
            { "I", HeMessageType.Information },
            { "W", HeMessageType.Warning },
            { "D", HeMessageType.Debug },
            { "Q", HeMessageType.Question }
        };
        #endregion

        #region 変数
        /// <summary>
        /// メッセージ
        /// </summary>
        private static Dictionary<string, HuXmlMessageInfo> messages = new Dictionary<string, HuXmlMessageInfo>();
        #endregion

        #region 静的なパブリック メソッド
        /// <summary>
        /// メッセージを取得する
        /// </summary>
        /// <param name="id">メッセージId</param>
        /// <returns>メッセージ</returns>
        public static HuXmlMessageInfo GetMessage(HecFwMessageId id)
        {
            var ret = default(HuXmlMessageInfo);
            var msgId = id.Str;

            if (messages.ContainsKey(msgId))
            {
                ret = messages[msgId];
                ret.Message = ret.XmlMessage;
            }
            return ret;
        }

        /// <summary>
        /// メッセージを取得する(パラメーターつき)
        /// </summary>
        /// <param name="id">メッセージId</param>
        /// <param name="parms">パラメーター</param>
        /// <returns>メッセージ</returns>
        public static HuXmlMessageInfo GetMessage(HecFwMessageId id, params object[] parms)
        {
            var ret = GetMessage(id);
            if (ret != null)
            {
                ret.Message = string.Format(ret.XmlMessage, parms);
            }
            return ret;
        }

        /// <summary>
        /// メッセージを取得する
        /// </summary>
        /// <param name="id">メッセージId</param>
        /// <returns>メッセージ</returns>
        public static string GetMessageText(HecFwMessageId id)
        {
            var ret = string.Empty;
            var mes = GetMessage(id);
            if (mes != null)
            {
                ret = mes.XmlMessage;
            }
            return ret;
        }

        /// <summary>
        /// メッセージを取得する(パラメーターつき)
        /// </summary>
        /// <param name="id">メッセージId</param>
        /// <param name="parms">パラメーター</param>
        /// <returns>メッセージ</returns>
        public static string GetMessageText(HecFwMessageId id, params object[] parms)
        {
            var ret = GetMessageText(id);
            if (HuString.IsNotEmpty(ret))
            {
                ret = string.Format(ret, parms);
            }
            return ret;
        }
        #endregion
    }
}
