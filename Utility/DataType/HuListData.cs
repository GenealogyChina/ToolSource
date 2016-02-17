using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.DataType
{
    /// <summary>
    /// リストデータクラス
    /// </summary>
    /// <remarks>
    /// <para>履歴:</para>
    /// <para>
    /// <list type="table">  
    /// <listheader>
    ///     <term>日付</term>
    ///     <term>更新者</term>
    ///     <term>パッケージ ID</term>
    ///     <description>説明</description>
    /// </listheader>  
    /// <item>
    ///     <term>2013/02/13</term>
    ///     <term>IBM福井</term>
    ///     <term>PCISP-1CSC-00001</term>
    ///     <description>新規作成</description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class HuListData : HuTextData
    {
        #region プロパティ用変数
        /// <summary>
        /// データ
        /// </summary>
        private bool _Selected = false;

        #endregion

        #region プロパティ
        /// <summary>
        /// 選択しているかどうか
        /// </summary>
        public bool Selected
        {
            get
            {
                return this._Selected;
            }

            set
            {
                this._Selected = value;
                onPropertyChanged(HuExpression.GetName(() => Selected));
            }
        }
        #endregion
    }
}
