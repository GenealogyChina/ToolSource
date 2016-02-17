using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Enum
{
    public class HecFwMessageId : HecEnum
    {
        #region 定数
        /// <summary>
        /// 型
        /// </summary>
        protected static readonly Type type = typeof(HecFwMessageId);

        /// <summary>
        /// 該当のファイルが存在しません。
        /// </summary>
        public static readonly HecFwMessageId MC0010CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0010CC));

        /// <summary>
        /// 指定ファイルを開けません。
        /// </summary>
        public static readonly HecFwMessageId MC0020CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0020CC));

        /// <summary>
        /// {0}に失敗しました。
        /// </summary>
        public static readonly HecFwMessageId MC0030CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0030CC));

        /// <summary>
        /// 入力情報が不十分なため、実行に失敗しました。
        /// </summary>
        public static readonly HecFwMessageId MC0040CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0040CC));

        /// <summary>
        /// {0}より小さい整数を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0050CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0050CC));

        /// <summary>
        /// {0}より大きい整数を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0060CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0060CC));

        /// <summary>
        /// {0}以下の整数を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0070CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0070CC));

        /// <summary>
        /// {0}以上の整数を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0080CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0080CC));

        /// <summary>
        /// {0}以上{1}以下で整数を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0090CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0090CC));

        /// <summary>
        /// {0}より大きく{1}より小さい整数を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0100CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0100CC));

        /// <summary>
        /// {0}より小さい数値を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0110CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0110CC));

        /// <summary>
        /// {0}より大きい数値を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0120CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0120CC));

        /// <summary>
        /// {0}と以上の数値を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0130CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0130CC));

        /// <summary>
        /// {0}と以下の数値を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0140CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0140CC));

        /// <summary>
        /// {0}以上{1}以下で数値を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0150CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0150CC));

        /// <summary>
        /// {0}より大きく{1}より小さい数値を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0160CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0160CC));

        /// <summary>
        /// 半角英字で入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0170CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0170CC));

        /// <summary>
        /// 半角カナで入力くしてください。
        /// </summary>
        public static readonly HecFwMessageId MC0180CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0180CC));

        /// <summary>
        /// 全角で入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0190CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0190CC));

        /// <summary>
        /// 全角カナで入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0200CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0200CC));

        /// <summary>
        /// 有効な形式({0})で入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0210CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0210CC));

        /// <summary>
        /// 有効な郵便番号形式（〒xxx-xxxx）で入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0220CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0220CC));

        /// <summary>
        /// 有効な携帯電話番号形式（xxx-xxxx-xxxx）で入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0230CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0230CC));

        /// <summary>
        /// 有効なメールアドレス（xxxx@xxxx）を入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0240CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0240CC));

        /// <summary>
        /// 有効な電話番号形式で入力してください。
        /// </summary>
        public static readonly HecFwMessageId MC0250CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0250CC));

        /// <summary>
        /// データベースに接続できません。
        /// </summary>
        public static readonly HecFwMessageId MC0260CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0260CC));

        /// <summary>
        /// 最低{0}文字を入力して下さい。
        /// </summary>
        public static readonly HecFwMessageId MC0270CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0270CC));

        /// <summary>
        /// 既に登録済みです。
        /// </summary>
        public static readonly HecFwMessageId MC0280CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0280CC));

        /// <summary>
        /// 参照モードのため、処理を実行できません。
        /// </summary>
        public static readonly HecFwMessageId MC0290CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0290CC));

        /// <summary>
        /// {0}の権限がありません。
        /// </summary>
        public static readonly HecFwMessageId MC0300CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC0300CC));

        /// <summary>
        /// 終了しますか？
        /// </summary>
        public static readonly HecFwMessageId MC4010CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC4010CC));

        /// <summary>
        /// {0}に設定します。よろしいですか？
        /// </summary>
        public static readonly HecFwMessageId MC4020CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC4020CC));

        /// <summary>
        /// {0}を削除します。よろしいですか？
        /// </summary>
        public static readonly HecFwMessageId MC4030CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC4030CC));

        /// <summary>
        /// 件数が{0}件を超えています。{1}件を表示します。
        /// </summary>
        public static readonly HecFwMessageId MC6010CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC6010CC));

        /// <summary>
        /// {0}が完了しました。
        /// </summary>
        public static readonly HecFwMessageId MC8010CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC8010CC));

        /// <summary>
        /// {0}を削除しました。
        /// </summary>
        public static readonly HecFwMessageId MC8020CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC8020CC));

        /// <summary>
        /// 件数： {0}件
        /// </summary>
        public static readonly HecFwMessageId MC8030CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC8030CC));

        /// <summary>
        /// 登録中です。しばらくお待ちください。
        /// </summary>
        public static readonly HecFwMessageId MC8040CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MC8040CC));

        /// <summary>
        /// {0}画面から{1}画面に遷移しました。
        /// </summary>
        public static readonly HecFwMessageId MD0010CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0010CC));

        /// <summary>
        /// {0}画面へ移動しています。
        /// </summary>
        public static readonly HecFwMessageId MD0020CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0020CC));

        /// <summary>
        /// {0}画面を開いています。
        /// </summary>
        public static readonly HecFwMessageId MD0030CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0030CC));

        /// <summary>
        /// {0}画面をポップアップしています。
        /// </summary>
        public static readonly HecFwMessageId MD0040CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0040CC));

        /// <summary>
        /// {0}画面をクローズしました。
        /// </summary>
        public static readonly HecFwMessageId MD0050CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0050CC));

        /// <summary>
        /// {0}コマンドを実行しています。
        /// </summary>
        public static readonly HecFwMessageId MD0060CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0060CC));

        /// <summary>
        /// {0}コマンドを実行しました。
        /// </summary>
        public static readonly HecFwMessageId MD0070CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0070CC));

        /// <summary>
        /// {0}サービスの{1}メソッドを実行しています。
        /// </summary>
        public static readonly HecFwMessageId MD0080CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0080CC));

        /// <summary>
        /// {0}サービスの{1}メソッドを実行し、{2}の结果を取得しました。
        /// </summary>
        public static readonly HecFwMessageId MD0090CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0090CC));

        /// <summary>
        /// {0}サービスの{1}メソッドを実行しました。
        /// </summary>
        public static readonly HecFwMessageId MD0100CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0100CC));

        /// <summary>
        /// {0}モックサービスの{1}メソッドを実行しています。
        /// </summary>
        public static readonly HecFwMessageId MD0110CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0110CC));

        /// <summary>
        /// {0}モックサービスの{1}メソッドを実行しました。
        /// </summary>
        public static readonly HecFwMessageId MD0120CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0120CC));

        /// <summary>
        /// {0}処理において例外が発生しました。{1}
        /// </summary>
        public static readonly HecFwMessageId MD0130CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0130CC));

        /// <summary>
        /// {0}画面を閉じるときに例外が発生しました。
        /// </summary>
        public static readonly HecFwMessageId MD0140CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0140CC));

        /// <summary>
        /// 該当のテスト入力データが見つかりません。
        /// </summary>
        public static readonly HecFwMessageId MD0150CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0150CC));

        /// <summary>
        /// ケース番号が存在しません。
        /// </summary>
        public static readonly HecFwMessageId MD0160CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0160CC));

        /// <summary>
        /// {0}が{1}コマンドを実行しました。
        /// </summary>
        public static readonly HecFwMessageId MD0170CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MD0170CC));

        /// <summary>
        /// ネットワーク通信エラーが発生しました。
        /// </summary>
        public static readonly HecFwMessageId MF0010CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MF0010CC));

        /// <summary>
        /// Serializable属性を持つオブジェクトである必要があります。
        /// </summary>
        public static readonly HecFwMessageId MF0040CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MF0040CC));

        /// <summary>
        /// Enum変換指定が間違っています(型：{0})。
        /// </summary>
        public static readonly HecFwMessageId MF0050CC = register<HecFwMessageId>(type, HuExpression.GetName(() =>
            MF0050CC));
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static HecFwMessageId() { }
        #endregion

        #region 静的なパブリック メソッド
        /// <summary>
        /// 有効化する
        /// </summary>
        public static void Activate() { }
        #endregion
    }
}
