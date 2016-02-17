using System;
using System.Collections.Generic;
using System.IO;
using ToolLibrary.Model.Core;

namespace ToolLibrary.Generator.Out.Service
{
    public class GenertorToMessage
    {
        private static GenertorToMessage genertor = null;
        public static void WriteOutPut(String path, HcMessageDivisionInfo info)
        {
            if(genertor == null)
            {
                genertor = new GenertorToMessage();
            }
            genertor.creatCustomerMessage(path, info.CustomerMessages);

            genertor.creatCommonMessage(path, info.CommonMessages);
        }

        private void creatCustomerMessage(string path, List<HcMessageInfo> messages)
        {
            creatCustomerMessageClass(path, messages);
            creatCustomerMessageXML(path, messages);
        }

        private void creatCustomerMessageClass(string path, List<HcMessageInfo> messages)
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


            var file = new FileStream(string.Format(@"{0}\GbMessageCode.java", folder.FullName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/*");
                writer.WriteLine(@" * {0}", Constants.ProjectName);
                writer.WriteLine(@" * {0}", Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".main.biz.constant;");
                writer.WriteLine(@"");
                writer.WriteLine(@"import " + Constants.Package + @".fw.core.message.MessageCode;");
                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * Biz层用MessageCode定义");
                writer.WriteLine(@" *");
                writer.WriteLine(@" * @author MXH");
                writer.WriteLine(@" */");
                writer.WriteLine(@"public interface GbMessageCode extends MessageCode {");
                writer.WriteLine(@"");
                foreach (var message in messages)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * {0}  ", message.Value);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"    public static final String {0} = ""{0}"";", message.ID.ToUpper());
                    writer.WriteLine(@"");
                }
                writer.WriteLine(@"}");
            }
        }

        private void creatCustomerMessageXML(string path, List<HcMessageInfo> messages)
        {
            string servicePath;
            servicePath = path + @"\Service\" + Constants.ProjectName;

            servicePath = servicePath + @"\src\message";

            if (Directory.Exists(servicePath))
            {
                Directory.Delete(servicePath, true);
            }

            var newFolder = Directory.CreateDirectory(servicePath);

            var file = new FileStream(string.Format(@"{0}\GbMessageCode.xml", newFolder.FullName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                writer.WriteLine(@"<msgs>");
                foreach (var message in messages)
                {
                    writer.WriteLine(@"    <msg id=""{0}"" type=""{1}"">{2}</msg>", message.ID.ToUpper(), message.Type.ToString(), message.Value);
                }
                writer.WriteLine(@"</msgs>");
            }
        }

        private void creatCommonMessage(String path, List<HcMessageInfo> commonMessages)
        {
            string servicePath;
            servicePath = path + @"\Service\" + Constants.Package + @".fw.core";

            servicePath = servicePath + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\fw\core\message";

            if (Directory.Exists(servicePath))
            {
                Directory.Delete(servicePath, true);
            }

            var newFolder = Directory.CreateDirectory(servicePath);

            var file = new FileStream(string.Format(@"{0}\MessageCode.java", newFolder.FullName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/*");
                writer.WriteLine(@" * {0}", Constants.ProjectName);
                writer.WriteLine(@" * {0}", Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".fw.core.message;");
                writer.WriteLine(@"");
                writer.WriteLine(@"");
                writer.WriteLine(@"public interface MessageCode {");
                writer.WriteLine(@"");
                foreach (var message in commonMessages)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * {0}  ", message.Value);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"    public static final String {0} = ""{0}"";", message.ID.ToUpper());
                    writer.WriteLine(@"");
                }
                writer.WriteLine(@"}");
            }
        }
    }
}
