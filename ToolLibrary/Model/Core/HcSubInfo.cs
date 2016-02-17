using System.Collections.Generic;

namespace ToolLibrary.Model.Core
{
    /// <summary>
    /// 子项目类
    /// </summary>
    public class HcSubInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Service
        /// </summary>
        public List<HcServiceInfo> ServiceArray { get; set; }

        /// <summary>
        /// Service
        /// </summary>
        public List<HcDTOInfo> DTOArray { get; set; }
    }
}
