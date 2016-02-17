using System.Collections.Generic;

namespace ToolLibrary.Model.Core
{
    /// <summary>
    /// 区分代码一览
    /// </summary>
    public class HcCtgCodeInfo
    {
        #region DB使用
        /// <summary>
        /// 区分定义
        /// </summary>
        public List<HcCodeInfo> Codes { get; set; }
        #endregion

        #region 画面表示用
        /// <summary>
        /// 区分定义
        /// </summary>
        public List<HcCodeInfo> SceneCodes { get; set; }
        #endregion

        public HcCtgCodeInfo()
        {
            Codes = new List<HcCodeInfo>();
            SceneCodes = new List<HcCodeInfo>();
        }
    }
}
