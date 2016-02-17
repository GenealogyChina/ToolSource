using System;
using System.IO;
using ToolLibrary.Model.Core;
using ToolLibrary.Type;
using Utility.Extension;

namespace ToolLibrary.Generator.Out.Service
{
    public class GenertorToService : IFExportSource
    {
        public void WriteOutPut(String path, HcSubInfo info)
        {
            string servicePath;
            servicePath = path + @"\Service\" + Constants.Package + @".main.service";

            servicePath = servicePath + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\main\service\" + info.Name.ToLower();

            if (Directory.Exists(servicePath))
            {
                Directory.Delete(servicePath, true);
            }

            servicePath = servicePath + @"\dto";

            var newFolder = Directory.CreateDirectory(servicePath);
            foreach (var service in info.ServiceArray)
            {
                creatInDTO(newFolder, service, info.Name.ToLower());

                createOutDTO(newFolder, service, info.Name.ToLower());

                createServiceInteface(newFolder.Parent, service, info.Name.ToLower());
            }

            string bizPath;
            bizPath = String.Format(@"{0}\Service\" + Constants.Package + @".main.biz\src\" + Constants.Package.Replace(@".", @"\") + @"\main\biz\" + info.Name.ToLower() + @"\dto", path);
            if (info.DTOArray != null)
            {
                foreach (var dto in info.DTOArray)
                {
                    writeDTO(bizPath, dto, info.Name.ToLower());
                }
            }
        }

        private void createServiceInteface(DirectoryInfo directoryInfo, HcServiceInfo service, String sub)
        {
            var file = new FileStream(string.Format(@"{0}\{1}.java", directoryInfo.FullName, service.Name), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/*");
                writer.WriteLine(@" * {0}", Constants.ProjectName);
                writer.WriteLine(@" * {0}", Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".main.service.{0};", sub);
                writer.WriteLine(@"");
                writer.WriteLine(@"import " + Constants.Package + @".fw.core.service.AbstractService;");
                if (service.ServiceType == EmServiceType.Update)
                {
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.type.Transactional;");
                }
                writer.WriteLine(@"import " + Constants.Package + @".main.service.{0}.dto.{1}InDTO;", sub, service.Name);
                writer.WriteLine(@"import " + Constants.Package + @".main.service.{0}.dto.{1}OutDTO;", sub, service.Name);
                writer.WriteLine(@"");
                writer.WriteLine(@"public class {0} extends AbstractService<{0}OutDTO, {0}InDTO> {{", service.Name);
                writer.WriteLine(@"");
                writer.WriteLine(@"	/*");
                writer.WriteLine(@"	 * (non-Javadoc)");
                writer.WriteLine(@"	 *");
                writer.WriteLine(@"	 * @see");
                writer.WriteLine(@"	 * " + Constants.Package + @".fw.core.service.AbstractService#execute(java.lang.Object)");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	@Override");
                if (service.ServiceType == EmServiceType.Update)
                {
                    writer.WriteLine(@"	@Transactional");
                }
                writer.WriteLine(@"	public {0}OutDTO execute(", service.Name);
                writer.WriteLine(@"			{0}InDTO {1}InDTO) {{", service.Name, service.Name.ToPrivateDefinition());
                writer.WriteLine(@"		{0}OutDTO {1}OutDTO = new {0}OutDTO();", service.Name, service.Name.ToPrivateDefinition());
                writer.WriteLine(@"		return {0}OutDTO;", service.Name.ToPrivateDefinition());
                writer.WriteLine(@"	}");
                writer.WriteLine(@"}");
            }
        }

        private void writeDTO(String path, HcDTOInfo info, String subName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var file = new FileStream(string.Format(@"{0}\{1}.java", path, info.Name), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * {0}", Constants.ProjectName);
                writer.WriteLine(@" * {0}", Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".main.biz." + subName + ".dto;");
                writer.WriteLine(@"");
                if (info.FieldArray != null && info.FieldArray.Count > 0)
                {
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.type.Alias;");
                }
                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * {0} DTO", info.Name);
                writer.WriteLine(@" *");
                writer.WriteLine(@" * History");
                writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                writer.WriteLine(@" * 1.0          {0}             TOOL                Create", DateTime.Now.ToString("yyyy/MM/dd"));
                writer.WriteLine(@" *");
                writer.WriteLine(@" */");
                writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.AliasKanJi(""{0}"")", info.Caption);
                writer.WriteLine(@"public class {0} implements java.io.Serializable, Comparable<{0}> {{", info.Name);
                writer.WriteLine(@"	/**");
                writer.WriteLine(@"	 * System ID");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	private static final long serialVersionUID = 1L;");
                writer.WriteLine(@"");

                foreach (var field in info.FieldArray)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * {0}  ", field.caption);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	@Alias(""{0}"")", field.caption);
                    writeFieldCheck(writer, field);
                    writer.WriteLine(@"	private {0} {1} = null;", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition());
                    writer.WriteLine(@"");
                }

                foreach (var field in info.FieldArray)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * <code>{0}</code>返回", field.caption);
                    writer.WriteLine(@"	 * @return <code>{0}</code>", field.caption);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public {0} get{1}() {{", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToTitleCase());
                    writer.WriteLine(@"		return this.{0};", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * <code>{0}</code>设定", field.name);
                    writer.WriteLine(@"	 * @param {0}　<code>{0}</code>设定值", field.name);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public void set{0}(", field.name.ToTitleCase());
                    writer.WriteLine(@"			{0} {1}) {{", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition());
                    writer.WriteLine(@"		this.{0} = {0};", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"");
                }

                writer.WriteLine(@"	/* (non-Javadoc)");
                writer.WriteLine(@"	 * @see java.lang.Comparable#compareTo(java.lang.Object)");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	@Override");
                writer.WriteLine(@"	public int compareTo({0} compareToDTO) {{", info.Name);
                writer.WriteLine(@"		return this.equals(compareToDTO) ? 0 : -1;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"}");
            }
        }

        private String getFieldTypeString(EmFieldType fieldType, String typeString)
        {
            switch (fieldType)
            {
                case EmFieldType.Boolean:
                    return "Boolean";
                case EmFieldType.BooleanArray:
                    return "Boolean[]";
                case EmFieldType.Byte:
                    return "byte";
                case EmFieldType.Date:
                    return "java.sql.Date";
                case EmFieldType.DateArray:
                    return "java.sql.Date[]";
                case EmFieldType.Double:
                    return "Double";
                case EmFieldType.DoubleArray:
                    return "Double[]";
                case EmFieldType.DTO:
                case EmFieldType.DTOArray:
                    return typeString;
                case EmFieldType.Integer:
                    return "Integer";
                case EmFieldType.IntegerArray:
                    return "Integer[]";
                case EmFieldType.Long:
                    return "Long";
                case EmFieldType.LongArray:
                    return "Long[]";
                case EmFieldType.String:
                    return "String";
                case EmFieldType.StringArray:
                    return "String[]";
                case EmFieldType.Time:
                    return "java.sql.Time";
                case EmFieldType.TimeArray:
                    return "java.sql.Time[]";
                case EmFieldType.Timestamp:
                    return "java.sql.Timestamp";
                case EmFieldType.TimestampArray:
                    return "java.sql.Timestamp[]";
                default:
                    return string.Empty;
            }
        }

        private void creatInDTO(DirectoryInfo newFolder, HcServiceInfo service, string infoName)
        {
            var file = new FileStream(string.Format(@"{0}\{1}InDTO.java", newFolder.FullName, service.Name), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/* {0}", Constants.ProjectName);
                writer.WriteLine(@"* {0}", Constants.CompanyName);
                writer.WriteLine(@"*/");
                writer.WriteLine(@"package " + Constants.Package + @".main.service.{0}.dto;", service.ModelID.ToLower());
                writer.WriteLine(@"");
                if (service.InDTO.FieldArray != null && service.InDTO.FieldArray.Count > 0)
                {
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.type.Alias;");
                }

                foreach (var item in service.InDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO || d.FieldType == EmFieldType.DTOArray))
                {
                    writer.WriteLine(@"import " + Constants.Package + @".main.biz.{0}.dto.{1};", infoName, item.FieldTypeString.Replace("[]", String.Empty));
                }

                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * {0}的InDTO", service.Caption);
                writer.WriteLine(@" *");
                writer.WriteLine(@" * 变更履历");
                writer.WriteLine(@" * REV.         变更日期           变更者              变更内容");
                writer.WriteLine(@" * -------      ---------------    ----------------    ------------------");
                writer.WriteLine(@" * 1.0          {0}         TOOL                初次作成", DateTime.Now.ToString("yyyy/MM/dd"));
                writer.WriteLine(@" *");
                writer.WriteLine(@" */");
                writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.AliasKanJi(""{0}"")", service.Caption);
                writer.WriteLine(@"public class {0} implements java.io.Serializable, Comparable<{0}> {{", service.InDTO.Name);
                writer.WriteLine(@"	/**");
                writer.WriteLine(@"	 * Serial ID");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	private static final long serialVersionUID = 1L;");
                writer.WriteLine(@"");

                foreach (var field in service.InDTO.FieldArray)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * {0}  ", field.caption);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	@Alias(""{0}"")", field.caption);
                    writeFieldCheck(writer, field);
                    writer.WriteLine(@"	private {0} {1} = null;", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition());
                    writer.WriteLine(@"");
                }

                foreach (var field in service.InDTO.FieldArray)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * <code>{0}</code>返回", field.caption);
                    writer.WriteLine(@"	 * @return <code>{0}</code>", field.caption);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public {0} get{1}() {{", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToTitleCase());
                    writer.WriteLine(@"		return this.{0};", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * <code>{0}</code>设定", field.name);
                    writer.WriteLine(@"	 * @param {0}　<code>{0}</code>设定值", field.name);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public void set{0}(", field.name.ToTitleCase());
                    writer.WriteLine(@"			{0} {1}) {{", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition());
                    writer.WriteLine(@"		this.{0} = {0};", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"");
                }

                writer.WriteLine(@"	/* (non-Javadoc)");
                writer.WriteLine(@"	 * @see java.lang.Comparable#compareTo(java.lang.Object)");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	@Override");
                writer.WriteLine(@"	public int compareTo({0} compareToDTO) {{", service.InDTO.Name);
                writer.WriteLine(@"		return this.equals(compareToDTO) ? 0 : -1;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"}");
            }
        }

        private void writeFieldCheck(StreamWriter writer, HcFieldInfo field)
        {
            if (field.FieldCheckType != null)
            {
                foreach (var item in field.FieldCheckType)
                {
                    switch (item)
                    {
                        case EmCheckType.MustEnter:
                            writer.WriteLine(@"	@" + Constants.Package + @".fw.core.validator.type.NotNull");
                            break;
                    }
                }
            }

        }

        private void createOutDTO(DirectoryInfo newFolder, HcServiceInfo service, String infoName)
        {
            var file = new FileStream(string.Format(@"{0}\{1}OutDTO.java", newFolder.FullName, service.Name), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/* {0}", Constants.ProjectName);
                writer.WriteLine(@"* {0}", Constants.CompanyName);
                writer.WriteLine(@"*/");
                writer.WriteLine(@"package " + Constants.Package + @".main.service.{0}.dto;", service.ModelID.ToLower());
                writer.WriteLine(@"");
                if (service.OutDTO.FieldArray != null && service.OutDTO.FieldArray.Count > 0)
                {
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.type.Alias;");
                }
                foreach (var item in service.OutDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO || d.FieldType == EmFieldType.DTOArray))
                {
                    writer.WriteLine(@"import " + Constants.Package + @".main.biz.{0}.dto.{1};", infoName, item.FieldTypeString.Replace("[]", String.Empty));
                }
                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * {0}的OutDTO", service.Caption);
                writer.WriteLine(@" *");
                writer.WriteLine(@" * 变更履历");
                writer.WriteLine(@" * REV.         变更日期           变更者              变更内容");
                writer.WriteLine(@" * -------      ---------------    ----------------    ------------------");
                writer.WriteLine(@" * 1.0          {0}         TOOL                初次作成", DateTime.Now.ToString("yyyy/MM/dd"));
                writer.WriteLine(@" *");
                writer.WriteLine(@" */");
                writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.AliasKanJi(""{0}"")", service.Caption);
                writer.WriteLine(@"public class {0} implements java.io.Serializable, Comparable<{0}> {{", service.OutDTO.Name);
                writer.WriteLine(@"	/**");
                writer.WriteLine(@"	 * Serial ID");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	private static final long serialVersionUID = 1L;");
                writer.WriteLine(@"");

                foreach (var field in service.OutDTO.FieldArray)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * {0}  ", field.caption);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	@Alias(""{0}"")", field.caption);
                    writeFieldCheck(writer, field);
                    writer.WriteLine(@"	private {0} {1} = null;", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition());
                    writer.WriteLine(@"");
                }

                foreach (var field in service.OutDTO.FieldArray)
                {
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * <code>{0}</code>返回", field.caption);
                    writer.WriteLine(@"	 * @return <code>{0}</code>", field.caption);
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public {0} get{1}() {{", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToTitleCase());
                    writer.WriteLine(@"		return this.{0};", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	/**");
                    writer.WriteLine(@"	 * <code>{0}</code>设定", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	 * @param {0}　<code>{0}</code>设定值", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	 */");
                    writer.WriteLine(@"	public void set{0}(", field.name.ToTitleCase());
                    writer.WriteLine(@"			{0} {1}) {{", getFieldTypeString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition());
                    writer.WriteLine(@"		this.{0} = {0};", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"");
                }

                writer.WriteLine(@"	/* (non-Javadoc)");
                writer.WriteLine(@"	 * @see java.lang.Comparable#compareTo(java.lang.Object)");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	@Override");
                writer.WriteLine(@"	public int compareTo({0} compareToDTO) {{", service.OutDTO.Name);
                writer.WriteLine(@"		return this.equals(compareToDTO) ? 0 : -1;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"}");
            }
        }

    }
}
