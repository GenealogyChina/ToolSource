using System;
using System.Collections.Generic;
using System.IO;
using ToolLibrary.Model.Core;

namespace ToolLibrary.Generator.Out.Service
{
    public class GenertorToCode
    {
        private static GenertorToCode genertor = null;
        public static void WriteOutPut(String path, HcCtgCodeInfo info)
        {
            if(genertor == null)
            {
                genertor = new GenertorToCode();
            }
            genertor.creatCtgCodeInfo(path, info.Codes);

            genertor.creatCtgCodeInfoForScene(path, info.SceneCodes);
        }

        private void creatCtgCodeInfo(string path, List<HcCodeInfo> codes)
        {
            string servicePath;
            servicePath = path + @"\Service\" + Constants.Package + @".main.biz";

            servicePath = servicePath + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\main\biz\constant";

            DirectoryInfo folder = null;
            if (!Directory.Exists(servicePath))
            {
                folder = Directory.CreateDirectory(servicePath);
            }
            else
            {
                folder = new DirectoryInfo(servicePath);
            }

            var file = new FileStream(string.Format(@"{0}\CtgCodeConstant.java", folder.FullName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/*");
                writer.WriteLine(@" * {0}", Constants.ProjectName);
                writer.WriteLine(@" * {0}", Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".main.biz.constant;");
                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * 类别管理class");
                writer.WriteLine(@" *");
                writer.WriteLine(@" * 变更履历");
                writer.WriteLine(@" * REV.         变更日期           变更者              变更内容");
                writer.WriteLine(@" * -------      ---------------    ----------------    ------------------");
                writer.WriteLine(@" * 1.0          {0}         MXH                 新版做成", DateTime.Now.ToString("yyyy/MM/dd"));
                writer.WriteLine(@" *");
                writer.WriteLine(@" */");
                writer.WriteLine(@"public interface CtgCodeConstant {");
                writer.WriteLine(@"");
                foreach (var code in codes)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * {0}  ", code.Name);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public static enum {0}_{1} {{", code.Table.ToString().ToUpper(), code.Column.ToString().ToUpper());
                    foreach (var item in code.Codes)
                    {
                        writer.WriteLine(@"		/**");
                        writer.WriteLine(@"		* {0}:{1}", item.Code, item.Caption);
                        writer.WriteLine(@"		*/");
                        writer.WriteLine(@"		{0} {{", item.ENName);
                        writer.WriteLine(@"			/* (non-Javadoc)");
                        writer.WriteLine(@"			 * @see java.lang.Enum#toString()");
                        writer.WriteLine(@"			 */");
                        writer.WriteLine(@"			@Override");
                        writer.WriteLine(@"            public String toString() {");
                        writer.WriteLine(@"				return ""{0}"";", item.Code);
                        writer.WriteLine(@"			}");
                        writer.WriteLine(@"        },");
                    }
                    writer.WriteLine(@"");
                }
                writer.WriteLine(@"}");
            }
        }

        private void creatCtgCodeInfoForScene(string path, List<HcCodeInfo> codes)
        {
            string servicePath;
            servicePath = path + @"\Service\" + Constants.Package + @".main.biz";

            servicePath = servicePath + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\main\biz\constant";

            DirectoryInfo folder = null;
            if (!Directory.Exists(servicePath))
            {
                folder = Directory.CreateDirectory(servicePath);
            }
            else
            {
                folder = new DirectoryInfo(servicePath);
            }

            var file = new FileStream(string.Format(@"{0}\CtgCodeForSceneConstant.java", folder.FullName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/*");
                writer.WriteLine(@" * {0}", Constants.ProjectName);
                writer.WriteLine(@" * {0}", Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".main.biz.constant;");
                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * 类别管理(画面用)class");
                writer.WriteLine(@" *");
                writer.WriteLine(@" * 变更履历");
                writer.WriteLine(@" * REV.         变更日期           变更者              变更内容");
                writer.WriteLine(@" * -------      ---------------    ----------------    ------------------");
                writer.WriteLine(@" * 1.0          {0}         MXH                 新版做成", DateTime.Now.ToString("yyyy/MM/dd"));
                writer.WriteLine(@" *");
                writer.WriteLine(@" */");
                writer.WriteLine(@"public interface CtgCodeForSceneConstant {");
                writer.WriteLine(@"");
                foreach (var code in codes)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * {0}  ", code.Name);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public static enum {0} {{", code.ENName.ToString().ToUpper());
                    foreach (var item in code.Codes)
                    {
                        writer.WriteLine(@"		/**");
                        writer.WriteLine(@"		* {0}:{1}", item.Code, item.Caption);
                        writer.WriteLine(@"		*/");
                        writer.WriteLine(@"		{0} {{", item.ENName);
                        writer.WriteLine(@"			/* (non-Javadoc)");
                        writer.WriteLine(@"			 * @see java.lang.Enum#toString()");
                        writer.WriteLine(@"			 */");
                        writer.WriteLine(@"			@Override");
                        writer.WriteLine(@"            public String toString() {");
                        writer.WriteLine(@"				return ""{0}"";", item.Code);
                        writer.WriteLine(@"			}");
                        writer.WriteLine(@"		},");
                    }
                    writer.WriteLine(@"");
                }
                writer.WriteLine(@"}");
            }
        }
    }
}
