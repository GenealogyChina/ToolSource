using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class HuNumber
    {
        #region 定数
        /// <summary>
        /// 開始インデックス値
        /// </summary>
        public const int StartIndex = 0;

        /// <summary>
        /// エラーインデックス値
        /// </summary>
        public const int ErrIndex = -1;
        #endregion

        #region 静的なパブリック メソッド
        /// <summary>
        /// 等しいかどうか(精度付き)
        /// </summary>
        /// <param name="v1">値その１</param>
        /// <param name="v2">値その２</param>
        /// <param name="order">有効桁数</param>
        /// <returns>等しい場合True</returns>
        public static bool EqualsWithPreCMZion(double v1, double v2, int order)
        {
            return GetNumberWithPreCMZion(v1, order) == GetNumberWithPreCMZion(v2, order);
        }

        /// <summary>
        /// 数値を取得する(精度付き)
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="order">有効桁数</param>
        /// <returns>値</returns>
        public static double GetNumberWithPreCMZion(double value, int order)
        {
            var dValue = Math.Pow(10, (((int)Math.Log10(value)) + 1 - order));
            return ((int)(value / dValue + 0.5)) * dValue;
        }

        /// <summary>
        /// パーセント値を標準化する
        /// </summary>
        /// <param name="percent">パーセント値</param>
        /// <returns>修正後の値</returns>
        public static double NormalizePercentage(double percent)
        {
            var ret = percent;
            if (ret < 0)
            {
                ret = 0;
            }
            else if (ret > 100)
            {
                ret = 100;
            }
            return ret;
        }
        #endregion
    }
}
