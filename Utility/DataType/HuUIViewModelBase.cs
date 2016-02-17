using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Utility.DataType
{
    /// <summary>
    /// UIデータ基底クラス
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
    public abstract class HuUIViewModelBase : INotifyPropertyChanged
    {
        #region イベント
        /// <summary>
        /// 属性変更のイベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region その他 メソッド
        /// <summary>
        /// 属性変更の処理
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
