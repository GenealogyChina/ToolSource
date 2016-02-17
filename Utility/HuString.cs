using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility
{
    public sealed class HuString
    {
        #region 定数
        /// <summary>
        /// 空文字列
        /// </summary>
        private const string EMPTY = "";

        /// <summary>
        /// スペース
        /// </summary>
        private const char SPACE_CHR = ' ';

        /// <summary>
        /// 全角スペース
        /// </summary>
        private const char ZENKAKU_SPACE_CHR = '　';

        /// <summary>
        /// ドライブ区切り子
        /// </summary>
        public const string DriveSeparetor = ":";

        /// <summary>
        /// ディレクトリ区切り子
        /// </summary>
        public const string DirSeparetor = "\\";

        /// <summary>
        /// ディレクトリ区切り子(UNIX系)
        /// </summary>
        public const string DirSeparetorUNIX = "/";
        #endregion

        #region 静的なパブリック メソッド
        /// <summary>
        /// 指定された文字列が数字かどうかを判別する
        /// </summary>
        /// <param name="value">対象文字</param>
        /// <returns>true: 数字, false: 数字でない</returns>
        public static bool IsNumberChar(string value)
        {
            return IsNotEmpty(value) ? Regex.IsMatch(value, @"^[0-9]+$") : false;
        }

        /// <summary>
        /// 指定された文字列が整数かどうかを判別する
        /// </summary>
        /// <param name="value">対象文字</param>
        /// <returns>true: 整数, false: 整数でない</returns>
        public static bool IsInteger(string value)
        {
            int dmy;
            return int.TryParse(value, out dmy);
        }

        /// <summary>
        /// 指定された文字列が数値かどうかを判別する
        /// </summary>
        /// <param name="value">対象文字</param>
        /// <returns>true: 数値, false: 数値でない</returns>
        public static bool IsNumeric(string value)
        {
            double dmy;
            return double.TryParse(value, out dmy);
        }

        /// <summary>
        /// 文字列の右側のホワイトスペースを削除する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <returns>変換後文字列</returns>
        public static string TrimRight(string value)
        {
            return IsNotEmpty(value) ? value.TrimEnd(SPACE_CHR) : EMPTY;
        }

        /// <summary>
        /// 文字列の左側のホワイトスペースを削除する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <returns>変換後文字列</returns>
        public static string TrimLeft(string value)
        {
            return IsNotEmpty(value) ? value.TrimStart(SPACE_CHR) : EMPTY;
        }

        /// <summary>
        /// 文字列の両側のホワイトスペースを削除する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <returns>変換後文字列</returns>
        public static string Trim(string value)
        {
            return IsNotEmpty(value) ? value.Trim(SPACE_CHR) : EMPTY;
        }

        /// <summary>
        /// 文字列の右側の全角および半角スペースを削除する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <returns>変換後文字列</returns>
        public static string TrimRightZ(string value)
        {
            return IsNotEmpty(value) ? value.TrimEnd() : EMPTY;
        }

        /// <summary>
        /// 文字列の左側の全角および半角スペースを削除する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <returns>変換後文字列</returns>
        public static string TrimLeftZ(string value)
        {
            return IsNotEmpty(value) ? value.TrimStart() : EMPTY;
        }

        /// <summary>
        /// 文字列の両側の全角および半角スペースを削除する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <returns>変換後文字列</returns>
        public static string TrimZ(string value)
        {
            return IsNotEmpty(value) ? value.Trim() : EMPTY;
        }

        /// <summary>
        /// XMLエスケープ変換する
        /// </summary>
        /// <param name="strIn">変換前文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string EscapeXml(string strIn)
        {
            if (IsNotEmpty(strIn))
            {
                strIn = strIn.Replace("&", "&amp;");
                strIn = strIn.Replace("<", "&lt;");
                strIn = strIn.Replace(">", "&gt;");
                strIn = strIn.Replace("\"", "&quot;");
                strIn = strIn.Replace("'", "&apos;");
            }
            return strIn;
        }

        /// <summary>
        /// 指定された文字列のバイト列長を取得する
        /// </summary>
        /// <param name="value">バイト列長を取得する対象の文字列</param>
        /// <returns>バイト列長</returns>
        public static int GetByteLength(string value)
        {
            return value != null ? HuEncoder.DefaultEncoding.GetByteCount(value) : 0;
        }

        /// <summary>
        /// 指定された文字列長を取得する
        /// </summary>
        /// <param name="value">対象の文字列</param>
        /// <returns>文字列長</returns>
        public static int GetLength(string value)
        {
            return IsNotEmpty(value) ? value.Length : 0;
        }

        /// <summary>
        /// 指定された文字列が空文字、NULLかを判断する
        /// </summary>
        /// <param name="value">対象の文字列</param>
        /// <returns>true: 空文字列、NULL値, false: 空文字,NULLではない</returns>
        public static bool IsEmpty(string value)
        {
            return value == null || value.Trim() == EMPTY;
        }

        /// <summary>
        /// 指定された文字列が空文字、NULLではないかを判断する
        /// </summary>
        /// <param name="value">対象の文字列</param>
        /// <returns>true: 空文字、NULLでない, false: 空文字列、NULL値</returns>
        public static bool IsNotEmpty(string value)
        {
            return !IsEmpty(value);
        }

        /// <summary>
        /// 指定された文字列は全て空文字、NULLかを判断する
        /// </summary>
        /// <param name="values">対象の文字列</param>
        /// <returns>true: 全てが空文字、NULL値</returns>
        public static bool IsAllEmpty(string[] values)
        {
            return values.Count(d => IsNotEmpty(d)) == 0;
        }

        /// <summary>
        /// 指定された文字列は全て空文字、NULLかを判断する
        /// </summary>
        /// <param name="value">対象の文字列</param>
        /// <param name="values">オプショナル対象の文字列</param>
        /// <returns>true: 全てが空文字、NULL値</returns>
        public static bool IsAllEmpty(string value, params string[] values)
        {
            return IsEmpty(value) && IsAllEmpty(values);
        }

        /// <summary>
        /// 指定された文字列のいずれかに空文字、NULLがあるかどうかを判断する
        /// </summary>
        /// <param name="values">対象の文字列</param>
        /// <returns>true: いずれかが空文字、NULL値がある</returns>
        public static bool IsAnyEmpty(string[] values)
        {
            return values.Count(d => IsEmpty(d)) > 0;
        }

        /// <summary>
        /// 指定された文字列いずれかが、空文字・NULLであるかどうかを判断する
        /// </summary>
        /// <param name="value">対象の文字列</param>
        /// <param name="values">オプショナル対象の文字列</param>
        /// <returns>true: いずれかが空文字かNULL値である場合</returns>
        public static bool IsAnyEmpty(string value, params string[] values)
        {
            return IsEmpty(value) || IsAnyEmpty(values);
        }

        /// <summary>
        /// 文字列を指定するタイプへ変換する
        /// </summary>
        /// <typeparam name="T">指定するタイプ</typeparam>
        /// <param name="value">文字列</param>
        /// <returns>変換结果</returns>
        public static T ConvertTo<T>(string value)
        {
            return IsNotEmpty(value) ? (T)Convert.ChangeType(value, typeof(T)) : default(T);
        }

        /// <summary>
        /// Object型をString型に変換する
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <returns>変換先オブジェクト</returns>
        public static string ObjectToString(object value)
        {
            return value != null ? value.ToString() : EMPTY;
        }

        /// <summary>
        /// 文字列の右側から指定文字で補完する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="size">最大サイズ</param>
        /// <param name="useChar">補完用文字</param>
        /// <returns>補完された文字列</returns>
        public static string PadRight(string value, int size, char useChar)
        {
            return value != null ? value.PadRight(size, useChar) : null;
        }

        /// <summary>
        /// 文字列の右側から空白文字で補完する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="size">最大サイズ</param>
        /// <returns>補完された文字列</returns>
        public static string PadRight(string value, int size)
        {
            return PadRight(value, size, SPACE_CHR);
        }

        /// <summary>
        /// 文字列の左側から指定文字で補完する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="size">最大サイズ</param>
        /// <param name="useChar">補完用文字</param>
        /// <returns>補完された文字列</returns>
        public static string PadLeft(string value, int size, char useChar)
        {
            return value != null ? value.PadLeft(size, useChar) : null;
        }

        /// <summary>
        /// 文字列の左側から空白文字で補完する
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="size">最大サイズ</param>
        /// <returns>補完された文字列</returns>
        public static string PadLeft(string value, int size)
        {
            return PadLeft(value, size, SPACE_CHR);
        }

        /// <summary>
        /// Base64でbyte配列をデコードする
        /// </summary>
        /// <param name="dataIn">文字列</param>
        /// <returns>デコードしたbyte配列</returns>
        public static byte[] DecodeBase64(string dataIn)
        {
            return IsNotEmpty(dataIn) ? Convert.FromBase64String(dataIn) : null;
        }

        /// <summary>
        /// Base64でbyte配列をエンコードする
        /// </summary>
        /// <param name="dataIn">byte配列</param>
        /// <returns>エンコードした文字列</returns>
        public static string EncodeBase64(byte[] dataIn)
        {
            return dataIn != null ? Convert.ToBase64String(dataIn) : EMPTY;
        }

        /// <summary>
        /// 16進法の文字列をbyte配列に変換する
        /// </summary>
        /// <param name="hexStringIn">hexStringIn</param>
        /// <returns>byte</returns>
        public static byte[] HexStringToBytes(string hexStringIn)
        {
            byte[] result = null;
            if (IsNotEmpty(hexStringIn) && hexStringIn.Length % 2 == 0)
            {
                var length = hexStringIn.Length / 2;
                result = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = Convert.ToByte(hexStringIn.Substring(i * 2, 2), 16);
                }
            }
            return result;
        }

        /// <summary>
        /// 大文字の文字列に変換する
        /// </summary>
        /// <param name="strIn">変換前の文字列</param>
        /// <returns>変換後の大文字の文字列</returns>
        public static string ToUpperCase(string strIn)
        {
            return strIn == null ? null : strIn.ToUpper();
        }

        /// <summary>
        /// 小文字の文字列に変換する
        /// </summary>
        /// <param name="strIn">変換前の文字列</param>
        /// <returns>変換後の小文字の文字列</returns>
        public static string ToLowerCase(string strIn)
        {
            return strIn == null ? null : strIn.ToLower();
        }

        /// <summary>
        /// 文字の順序を逆にする
        /// </summary>
        /// <param name="strIn">変換前の文字列</param>
        /// <returns>順序が逆になった文字列</returns>
        public static string MakeReverse(string strIn)
        {
            string strRet = null;
            if (IsNotEmpty(strIn))
            {
                var charArray = strIn.ToCharArray();
                Array.Reverse(charArray);
                strRet = new string(charArray);
            }
            return strRet;
        }

        /// <summary>
        /// 指定文字列全てが全角文字列かどうかを判定する
        /// </summary>
        /// <param name="strMoji">判定対象文字列</param>
        /// <returns>全角文字列の場合True</returns>
        public static bool IsZenkaku(string strMoji)
        {
            return strMoji != null && GetByteLength(strMoji) == strMoji.Length * 2;
        }

        /// <summary>
        /// 指定文字列全てが半角文字列かどうかを判定する
        /// </summary>
        /// <param name="strMoji">判定対象文字列</param>
        /// <returns>半角文字列の場合True</returns>
        public static bool IsHankaku(string strMoji)
        {
            return strMoji != null && GetByteLength(strMoji) == strMoji.Length;
        }

        /// <summary>
        /// 値を追加する(区切り子つき)
        /// </summary>
        /// <param name="sb">ストリングビルダー</param>
        /// <param name="value">値</param>
        /// <param name="separator">区切り子</param>
        public static void Append(StringBuilder sb, string value, string separator = ", ")
        {
            if (HuString.IsNotEmpty(value))
            {
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(Trim(value));
            }
        }

        /// <summary>
        /// キー・値から文字列を追加する
        /// </summary>
        /// <param name="sb">ストリングビルダー</param>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <param name="separator">区切り子</param>
        /// <param name="format">文字列作成のフォーマット</param>
        public static void AppendKeyValueText(StringBuilder sb, string key, string value, string separator = ", ",
             string format = "{0}: {1}")
        {
            if (HuString.IsNotEmpty(key) && HuString.IsNotEmpty(value))
            {
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }
                sb.AppendFormat(format, Trim(key), Trim(value));
            }
        }

        /// <summary>
        /// 値を追加する(区切り子つき)
        /// </summary>
        /// <param name="text">ベース</param>
        /// <param name="value">値</param>
        /// <param name="separator">区切り子</param>
        public static string GetAppendText(string text, string value, string separator = ", ")
        {
            var ret = text;
            if (HuString.IsNotEmpty(value))
            {
                if (ret.Length > 0)
                {
                    ret += separator;
                }
                ret += Trim(value);
            }
            return ret;
        }

        /// <summary>
        /// テキストを分離する
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="separetor">区切り子</param>
        /// <param name="bForward">前方からかどうか</param>
        /// <returns>分離されたテキストのリスト</returns>
        public static List<string> SepareteText(string text, string separetor, bool bForward = true)
        {
            var ret = new List<string>();
            if (IsNotEmpty(text) && separetor != null && separetor.Length > 0)
            {
                var index = bForward ? text.IndexOf(separetor) : text.LastIndexOf(separetor);
                if (index != HuNumber.ErrIndex)
                {
                    ret.Add(text.Substring(HuNumber.StartIndex, index));
                    ret.Add(text.Substring(index + separetor.Length));
                }
                else
                {
                    ret.Add(text);
                }
            }
            return ret;
        }

        /// <summary>
        /// テキストを結合する
        /// </summary>
        /// <param name="preText">前テキスト</param>
        /// <param name="postText">後ろテキスト</param>
        /// <param name="separetor">結合文字</param>
        /// <returns>結合したテキスト</returns>
        public static string JointText(string preText, string postText, string separetor)
        {
            var ret = string.Empty;
            if (IsEmpty(preText))
            {
                ret = postText;
            }
            else if (IsEmpty(postText))
            {
                ret = preText;
            }
            else
            {
                if (preText.EndsWith(separetor))
                {
                    preText = preText.Substring(HuNumber.StartIndex, preText.Length - separetor.Length);
                }
                if (postText.StartsWith(separetor))
                {
                    postText = postText.Substring(separetor.Length);
                }
                ret = preText + separetor + postText;
            }

            return ret;
        }

        /// <summary>
        /// 複数テキストを結合する
        /// </summary>
        /// <param name="texts">テキストのリスト</param>
        /// <param name="separetor">結合文字</param>
        /// <returns>結合したテキスト</returns>
        public static string JointText(List<string> texts, string separetor)
        {
            var ret = string.Empty;
            foreach (var text in texts)
            {
                ret = JointText(ret, text, separetor);
            }
            return ret;
        }

        /// <summary>
        /// キー・値から文字列を追加する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <param name="format">文字列作成のフォーマット</param>
        public static string GetKeyValueText(string key, string value, string format = "{0}: {1}")
        {
            var ret = string.Empty;
            if (HuString.IsNotEmpty(key) && HuString.IsNotEmpty(value))
            {
                ret = string.Format(format, Trim(key), Trim(value));
            }
            return ret;
        }

        /// <summary>
        /// 値かnullを返す
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>値かnull</returns>
        public static string ValueOrNull(string value)
        {
            return IsNotEmpty(value) ? value : null;
        }

        /// <summary>
        /// 等しいかキーが空かを返す
        /// </summary>
        /// <param name="value">調査対象</param>
        /// <param name="key">キー</param>
        /// <returns>キーと等しいかキーが空の場合True</returns>
        public static bool IsEqualOrEmpty(string value, string key)
        {
            return IsEmpty(key) || value == key;
        }

        /// <summary>
        /// 含まれるかキーが空かを返す
        /// </summary>
        /// <param name="value">調査対象</param>
        /// <param name="key">キー</param>
        /// <returns>キーが含まれる場合かキーが空の場合True</returns>
        public static bool IsContainsOrEmpty(string value, string key)
        {
            return IsEmpty(key) || value.Contains(key);
        }

        /// <summary>
        /// 指定されたバイト数分の文字を取得する
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="byteCount">取得するバイト数</param>
        /// <param name="bForward">破棄する方向が前方であるかどうか</param>
        /// <returns>文字列</returns>
        public static string GetLtdText(string target, int byteCount, bool bForward = false)
        {
            var ret = target;
            var i = 0;
            var cnt = 0;
            if (target != null)
            {
                if (!bForward)
                {
                    for (i = 0; i < target.Length; i++)
                    {
                        cnt += GetByteLength(target.Substring(i, 1));
                        if (cnt > byteCount)
                        {
                            break;
                        }
                    }
                    ret = target.Substring(0, i);
                }
                else
                {
                    for (i = target.Length - 1; i >= 0; i--)
                    {
                        cnt += GetByteLength(target.Substring(i, 1));
                        if (cnt > byteCount)
                        {
                            break;
                        }
                    }
                    ret = target.Substring(i + 1);
                }
            }
            return ret;
        }
        #endregion
    }
}
