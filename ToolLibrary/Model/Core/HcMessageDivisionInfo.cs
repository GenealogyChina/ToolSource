using System.Collections.Generic;

namespace ToolLibrary.Model.Core
{
    /// <summary>
    /// 信息区分 类信息
    /// </summary>
    public class HcMessageDivisionInfo
    {
        /// <summary>
        /// 自定义消息列表
        /// </summary>
        public List<HcMessageInfo> CustomerMessages { get; set; }

        /// <summary>
        /// 共通消息一览
        /// </summary>
        public List<HcMessageInfo> CommonMessages { get; set; }

        /// <summary>
        /// 客户端消息一览
        /// </summary>
        public List<HcMessageInfo> ClientMessages { get; set; }
    }
}
