using System.Collections.Generic;
using ToolLibrary.Type;

namespace ToolLibrary.Model.Core
{
    /// <summary>
    /// 字段信息
    /// </summary>
    public class HcFieldInfo
    {
        /// <summary>
        /// 项目物理名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string caption { get; set; }

        /// <summary>
        /// 项目数据类型
        /// </summary>
        public EmFieldType FieldType { get; private set; }

        private string _FieldTypeString;

        /// <summary>
        /// 项目数据类型名
        /// </summary>
        public string FieldTypeString
        {
            get
            {
                return this._FieldTypeString;
            }
            set
            {
                this._FieldTypeString = value;
                switch (value)
                {
                    case "文字":
                        this.FieldType = EmFieldType.String;
                        break;
                    case "文字[]":
                        this.FieldType = EmFieldType.StringArray;
                        break;
                    case "比特[]":
                        this.FieldType = EmFieldType.Byte;
                        break;
                    case "整数":
                        this.FieldType = EmFieldType.Integer;
                        break;
                    case "整数[]":
                        this.FieldType = EmFieldType.IntegerArray;
                        break;
                    case "长整数":
                        this.FieldType = EmFieldType.Long;
                        break;
                    case "长整数[]":
                        this.FieldType = EmFieldType.LongArray;
                        break;
                    case "实数":
                        this.FieldType = EmFieldType.Double;
                        break;
                    case "实数[]":
                        this.FieldType = EmFieldType.DoubleArray;
                        break;
                    case "真假":
                        this.FieldType = EmFieldType.Boolean;
                        break;
                    case "真假[]":
                        this.FieldType = EmFieldType.BooleanArray;
                        break;
                    case "日期":
                        this.FieldType = EmFieldType.Date;
                        break;
                    case "日期[]":
                        this.FieldType = EmFieldType.DateArray;
                        break;
                    case "时间":
                        this.FieldType = EmFieldType.Timestamp;
                        break;
                    case "时间[]":
                        this.FieldType = EmFieldType.TimestampArray;
                        break;
                    case "时刻":
                        this.FieldType = EmFieldType.Time;
                        break;
                    case "时刻[]":
                        this.FieldType = EmFieldType.TimeArray;
                        break;
                    default:
                        var strType1 = string.Empty;
                        var strType2 = string.Empty;
                        if (value.Length > 3)
                        {
                            strType1 = value.Substring(value.Length - 3, 3);
                        }

                        if (value.Length > 5)
                        {
                            strType2 = value.Substring(value.Length - 5, 5);
                        }

                        if (strType1.Equals("DTO"))
                        {
                            this.FieldType = EmFieldType.DTO;
                        }
                        else if (strType2.Equals("DTO[]"))
                        {
                            this.FieldType = EmFieldType.DTOArray;
                        }
                        break;
                }
            }
        }

        private List<EmCheckType> _FieldCheckType;

        /// <summary>
        /// 项目输入验证
        /// </summary>
        public List<EmCheckType> FieldCheckType
        {
            get
            {
                return this._FieldCheckType;
            }

            set
            {
                this._FieldCheckType = value;
            }
        }
    }
}
