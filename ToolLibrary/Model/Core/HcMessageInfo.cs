using ToolLibrary.Type;

namespace ToolLibrary.Model.Core
{
    /// <summary>
    /// 信息 类信息
    /// </summary>
    public class HcMessageInfo
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 消息种类
        /// </summary>
        public EmMessageType Type { get; set; }

        /// <summary>
        /// skip属性
        /// </summary>
        public EmSkipType? Skip { get; set; }
    }
}
