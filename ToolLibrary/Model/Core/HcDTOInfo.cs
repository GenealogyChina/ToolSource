using System.Collections.Generic;

namespace ToolLibrary.Model.Core
{
    /// <summary>
    /// DTO 类信息
    /// </summary>
    public class HcDTOInfo
    {
        /// <summary>
        /// 对象物理名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public List<HcFieldInfo> FieldArray { get; set; }
    }
}
