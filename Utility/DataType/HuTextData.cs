using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.DataType
{
    /// <summary>
    /// 名前、データクラス
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
    public class HuTextData : HuUIViewModelBase
    {
        #region 変数

        #endregion

        #region プロパティ用変数
        /// <summary>
        /// 編集可能かどうか
        /// </summary>
        private bool _Editable;

        /// <summary>
        /// データ
        /// </summary>
        private object _Data;

        private string _Text;
        #endregion

        #region プロパティ
        /// <summary>
        /// 表示文字列プロパティ
        /// </summary>
        public string Text
        {
            get
            {
                return this._Text;
            }

            set
            {
                this._Text = value;
                onPropertyChanged(HuExpression.GetName(() => Text));
            }
        }


        /// <summary>
        /// 編集可能かどうか
        /// </summary>
        public bool Editable
        {
            get
            {
                return this._Editable;
            }

            set
            {
                this._Editable = value;
                onPropertyChanged(HuExpression.GetName(() => Editable));
            }
        }

        /// <summary>
        /// データプロパティ
        /// </summary>
        public object Data
        {
            get
            {
                return this._Data;
            }

            set
            {
                this._Data = value;
                onPropertyChanged(HuExpression.GetName(() => Data));
            }
        }
        #endregion
    }
}
