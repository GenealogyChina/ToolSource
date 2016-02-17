using ToolLibrary.Type;

namespace ToolLibrary.Model.Core
{
    /// <summary>
    /// Service信息类
    /// </summary>
    public class HcServiceInfo
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModelID { get; set; }

        /// <summary>
        /// Service物理名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Service名称
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Service类型
        /// </summary>
        public EmServiceType ServiceType { get; set; }

        /// <summary>
        /// Service处理概要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Service的InDTO
        /// </summary>
        public HcDTOInfo InDTO { get; set; }

        /// <summary>
        /// Service的OutDTO
        /// </summary>
        public HcDTOInfo OutDTO { get; set; }
    }
}
