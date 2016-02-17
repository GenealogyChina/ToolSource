using System.Collections.Generic;

namespace ToolLibrary.Model.Core
{
    public class HcCodeInfo
    {
        /// <summary>
        /// 区分名（中文名）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区分名（英语名）
        /// </summary>
        public string ENName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 区分定义
        /// </summary>
        public List<HcCodeItemInfo> Codes { get; set; }

        public HcCodeInfo()
        {
            Codes = new List<HcCodeItemInfo>();
        }
    }
}
