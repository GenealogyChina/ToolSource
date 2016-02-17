using Utility.Enum.Utility;

namespace Utility
{
    public class HuMessageInfo
    {
        #region プロパティ
        /// <summary>
        /// メッセージ種別
        /// </summary>
        public HeMessageType Type { get; set; }

        /// <summary>
        /// メッセージID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// メッセージ内容
        /// </summary>
        public string Message { get; set; }
        #endregion
    }
}
