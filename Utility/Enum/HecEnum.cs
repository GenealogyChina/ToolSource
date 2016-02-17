using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Enum
{
    public class HecEnum
    {
        #region 変数
        /// <summary>
        /// 列挙体カウンター
        /// </summary>
        private static Dictionary<Type, int> countMaster = new Dictionary<Type, int>();

        /// <summary>
        /// 列挙体情報
        /// </summary>
        private static Dictionary<Type, List<HecEnum>> enumList = new Dictionary<Type, List<HecEnum>>();
        #endregion

        #region プロパティ用変数
        /// <summary>
        /// カウンターの値(文字列表現)
        /// </summary>
        private string _NumStr;
        #endregion

        #region プロパティ
        /// <summary>
        /// カウンターの値
        /// </summary>
        public int Num { get; private set; }

        /// <summary>
        /// カウンターの値(文字列表現)
        /// </summary>
        public string NumStr
        {
            get
            {
                if (this._NumStr == null)
                {
                    this._NumStr = this.Num.ToString();
                }
                return this._NumStr;
            }
        }

        /// <summary>
        /// 文字識別子
        /// </summary>
        public string Str { get; private set; }
        #endregion

        #region 静的なパブリック メソッド
        /// <summary>
        /// 定義された列挙体であるか
        /// </summary>
        /// <param name="type">型</param>
        /// <param name="str">識別子</param>
        /// <returns>定義されている場合True</returns>
        public static bool IsDefined(Type type, string str)
        {
            return enumList.ContainsKey(type) && enumList[type].FirstOrDefault(d => d.Str == str || d.NumStr == str) !=
                null;
        }

        /// <summary>
        /// 列挙体に変換試行する
        /// </summary>
        /// <typeparam name="T">列挙体の型</typeparam>
        /// <param name="str">文字</param>
        /// <param name="eOut">変換後の列挙体</param>
        /// <returns>変換できた場合True</returns>
        public static bool TryParse<T>(string str, out T eOut) where T : HecEnum
        {
            var type = typeof(T);
            var ret = IsDefined(type, str);
            eOut = null;
            if (ret)
            {
                eOut = enumList[type].FirstOrDefault(d => d.Str == str || d.NumStr == str) as T;
            }
            return ret;
        }
        #endregion

        #region オーバーライド　パブリック　メソッド
        /// <summary>
        /// 文字列表現を返す
        /// </summary>
        /// <returns>文字列表現</returns>
        public override string ToString()
        {
            return this.Str;
        }

        /// <summary>
        /// 等しいかどうか
        /// </summary>
        /// <param name="obj">入力値</param>
        /// <returns>等しい場合True</returns>
        public override bool Equals(object obj)
        {
            var ret = false;
            if (obj != null && this.GetType() == obj.GetType())
            {
                ret = this == (HecEnum)obj;
            }
            return ret;
        }

        /// <summary>
        /// ハッシュ値を返す
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            return this.Num ^ this.Str.GetHashCode();
        }
        #endregion

        #region 静的なその他 メソッド
        /// <summary>
        /// 列挙体登録(自動カウント)
        /// </summary>
        /// <param name="type">まとまりとしたい型</param>
        /// <param name="str">識別子</param>
        /// <typeparam name="T">列挙体の型</typeparam>
        /// <returns>列挙体インスタンス</returns>
        protected static T register<T>(Type type, string str) where T : HecEnum, new()
        {
            var count = 0;
            if (countMaster.ContainsKey(type))
            {
                count = countMaster[type];
            }
            return register<T>(type, str, count);
        }

        /// <summary>
        /// 列挙体登録(カウンター値指定)
        /// </summary>
        /// <param name="type">まとまりとしたい型</param>
        /// <param name="str">識別子</param>
        /// <param name="count">カウンター値</param>
        /// <typeparam name="T">列挙体の型</typeparam>
        /// <returns>列挙体インスタンス</returns>
        protected static T register<T>(Type type, string str, int count) where T : HecEnum, new()
        {
            var ret = new T() { Str = str, Num = count++ };
            if (countMaster.ContainsKey(type))
            {
                countMaster[type] = count;
                enumList[type].Add(ret);
            }
            else
            {
                countMaster.Add(type, count);
                enumList.Add(type, new List<HecEnum>() { ret });
            }
            return ret;
        }

        /// <summary>
        /// ==演算子のオーバーロード
        /// </summary>
        /// <param name="v1">値その１</param>
        /// <param name="v2">値その２</param>
        /// <returns>等しい場合True</returns>
        public static bool operator ==(HecEnum v1, HecEnum v2)
        {
            var ret = false;
            if (object.Equals(v1, null) && object.Equals(v2, null))
            {
                ret = true;
            }
            else if (!object.Equals(v1, null) && !object.Equals(v2, null))
            {
                ret = v1.Str == v2.Str && v1.Num == v2.Num;
            }
            return ret;
        }

        /// <summary>
        /// !=演算子のオーバーロード
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>等しくない場合True</returns>
        public static bool operator !=(HecEnum v1, HecEnum v2)
        {
            var ret = false;
            if ((object.Equals(v1, null) && !object.Equals(v2, null)) || (!object.Equals(v1, null) &&
                object.Equals(v2, null)))
            {
                ret = true;
            }
            else if (!object.Equals(v1, null) && !object.Equals(v2, null))
            {
                ret = !(v1.Str == v2.Str && v1.Num == v2.Num);
            }
            return ret;
        }
        #endregion
    }
}
