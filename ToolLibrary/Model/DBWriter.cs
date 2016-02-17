using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolLibrary.Model.Core;
using Utility.Extension;

namespace ToolLibrary.Model
{
    public class DBWriter
    {
        public void WriteClientDTO(string path, List<HcTableInfo> tables, List<HcViewInfo> views)
        {
            var newFolder = Directory.CreateDirectory(path + @"\C#\dto");
            List<HcDBBaseInfo> lst = new List<HcDBBaseInfo>();
            lst.AddRange(tables);
            lst.AddRange(views);

            foreach (var info in lst)
            {
                var file = new FileStream(string.Format(@"{0}\{1}InfoDTO.cs", newFolder.FullName, info.Name.ToTitleCase()), FileMode.Create, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"using CodeMZ.CMZ.Fw.Utility.Model;");
                    writer.WriteLine(@"using CodeMZ.CMZ.Sol.SDTO.Table;");
                    writer.WriteLine(@"using System;");
                    writer.WriteLine(@"using System.Collections.Generic;");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"namespace CodeMZ.CMZ.Sol.SDTO.COM");
                    writer.WriteLine(@"{");
                    writer.WriteLine(@"    /// <summary>");
                    writer.WriteLine(@"    /// ");
                    writer.WriteLine(@"    /// </summary>");
                    writer.WriteLine(@"    /// <remarks>");
                    writer.WriteLine(@"    /// <para>History:</para>");
                    writer.WriteLine(@"    /// <para>");
                    writer.WriteLine(@"    /// <list type=""table"">");
                    writer.WriteLine(@"    /// <listheader>");
                    writer.WriteLine(@"    ///     <term>Update Date</term>");
                    writer.WriteLine(@"    ///     <term>Updater</term>");
                    writer.WriteLine(@"    ///     <description>Infomation</description>");
                    writer.WriteLine(@"    /// </listheader>");
                    writer.WriteLine(@"    /// <item>");
                    writer.WriteLine(@"    ///     <term> {0} </term>", DateTime.Now.ToString("yyyy-MM-dd"));
                    writer.WriteLine(@"    ///     <term> {0}</term>", ToolLibrary.Constants.ProjectName);
                    writer.WriteLine(@"    ///     <description> Create</description>");
                    writer.WriteLine(@"    /// </item>");
                    writer.WriteLine(@"    /// </list>");
                    writer.WriteLine(@"    /// </para>");
                    writer.WriteLine(@"    /// </remarks>");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"    public class Hs{0}DTO : HsServiceDTOBase", info.Name);
                    writer.WriteLine(@"    {");
                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"        /// <summary>");
                        writer.WriteLine(@"        /// {0}", column.ColumnName);
                        writer.WriteLine(@"        /// <summary>");
                        writer.WriteLine(@"        public {0} {1} {{ get; set; }}", getCshapTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"");
                    }
                    writer.WriteLine(@"        /// <summary>");
                    writer.WriteLine(@"        /// parmary init");
                    writer.WriteLine(@"        /// <summary>");
                    writer.WriteLine(@"        protected override void initMember()");
                    writer.WriteLine(@"        {");
                    foreach (var column in info.Columns.FindAll(d => d.DataType != MySqlDbType.Int32))
                    {
                        writer.WriteLine(@"            {0} = string.Empty;", column.ColumnName.ToPrivateDefinition());
                    }
                    writer.WriteLine(@"        }");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"        /// <summary>");
                    writer.WriteLine(@"        /// The String Fomate Data value of Null");
                    writer.WriteLine(@"        /// <summary>");
                    writer.WriteLine(@"        protected override void setNullForString()");
                    writer.WriteLine(@"        {");
                    foreach (var column in info.Columns.FindAll(d => d.DataType != MySqlDbType.Int32))
                    {
                        writer.WriteLine(@"            {0} = null;", column.ColumnName.ToPrivateDefinition());
                    }
                    writer.WriteLine(@"        }");
                    writer.WriteLine(@"    }");
                    writer.WriteLine(@"}");
                }
            }
        }

        public void WriteServiceBiz(string path, List<HcTableInfo> tables, List<HcViewInfo> views)
        {
            var newFolder = Directory.CreateDirectory(path + @"\Service\" + Constants.Package + @".main.biz\dto");
            List<HcDBBaseInfo> lst = new List<HcDBBaseInfo>();
            lst.AddRange(tables);
            lst.AddRange(views);
            foreach (var info in lst)
            {
                var file = new FileStream(string.Format(@"{0}\{1}InfoDTO.java", newFolder.FullName, info.Name.ToTitleCase()), FileMode.Create, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"/*");
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.ProjectName);
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.CompanyName);
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"package " + Constants.Package + @".main.biz.com.dto;");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.type.Alias;");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"/**");
                    writer.WriteLine(@" *Biz DTO");
                    writer.WriteLine(@" *");
                    writer.WriteLine(@" * History");
                    writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                    writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                    writer.WriteLine(@" * 1.0          {0}             TOOL                 Create", DateTime.Now.ToString("yyyy/MM/dd"));
                    writer.WriteLine(@"*");
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.AliasKanJi("""")");
                    writer.WriteLine(@"public class {0}InfoDTO implements java.io.Serializable, Comparable<{0}InfoDTO> {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	private static final long serialVersionUID = 1L;");

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	private {0} {1} = null;", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                    }

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public {0} get{1}() {{", getJavaTypeString(column.DataType), column.ColumnName.ToPublicDefinition());
                        writer.WriteLine(@"		return this.{0};", column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"	}");
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public void set{0}(", column.ColumnName.ToPublicDefinition());
                        writer.WriteLine(@"			{0} {1}) {{", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"		this.{0} = {0};", column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"	}");
                    }
                    writer.WriteLine(@"");
                    writer.WriteLine(@"  	/* (non-Javadoc)");
                    writer.WriteLine(@"     * @see java.lang.Comparable#compareTo(java.lang.Object)");
                    writer.WriteLine(@"     */");
                    writer.WriteLine(@"    @Override");
                    writer.WriteLine(@"    public int compareTo(");
                    writer.WriteLine(@"			{0}InfoDTO compareToDTO) {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"        return this.equals(compareToDTO) ? 0 : -1;");
                    writer.WriteLine(@"    }");
                    writer.WriteLine(@"}");
                    writer.WriteLine(@"");
                }
            }
        }

        public void WriteServiceDB(string path, List<HcTableInfo> tables, List<HcViewInfo> views)
        {
            var newFolder = Directory.CreateDirectory(path + @"\Service\" + Constants.Package + @".main.db");
            createMyBatis(newFolder.FullName, tables, views);
            createTblJava(newFolder.FullName, tables);
            createViewJava(newFolder.FullName, views);
        }

        public void WriteServiceDBSql(string path, HcSQLInfo sqlInfo)
        {
            DirectoryInfo dir;
            if (Directory.Exists(path + @"\" + Constants.Package + @".main.db"))
            {
                dir = new DirectoryInfo(path + @"\" + Constants.Package + @".main.db");
            }
            else
            {
                dir = Directory.CreateDirectory(path + @"\" + Constants.Package + @".main.db");
            }
            createMybatisCustomXML(dir.FullName, sqlInfo);
            createSqlJava(dir.FullName, sqlInfo);
        }

        private void createMybatisCustomXML(string folder, HcSQLInfo sqlInfo)
        {
            DirectoryInfo newFolder;
            if (Directory.Exists(folder + @"\src\sql\cmt"))
            {
                newFolder = new DirectoryInfo(folder + @"\src\sql\cmt");
            }
            else
            {
                newFolder = Directory.CreateDirectory(folder + @"\src\sql\cmt");
            }

            var file = new FileStream(string.Format(@"{0}\custom.xml", newFolder.FullName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>");
                writer.WriteLine(@"<!DOCTYPE mapper PUBLIC ""-//mybatis.org//DTD Mapper 3.0//EN"" ""http://mybatis.org/dtd/mybatis-3-mapper.dtd"">");
                writer.WriteLine(@"");
                writer.WriteLine(@"<mapper namespace=""custom"">");
                writer.WriteLine(@"");
                writer.WriteLine(@"	<select id=""{0}"" resultType=""{1}.main.db.sql.{2}DTO"">", sqlInfo.Key, Constants.Package, sqlInfo.Name.ToTitleCase());
                writer.WriteLine(@"		{0}", sqlInfo.SQL);
                writer.WriteLine(@"	</select>");
                writer.WriteLine(@"</mapper>");
            }
        }


        private void createSqlJava(string folder, HcSQLInfo sqlInfo)
        {
            DirectoryInfo newFolder;
            if (Directory.Exists(folder + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\main\db\sql"))
            {
                newFolder = new DirectoryInfo(folder + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\main\db\sql");
            }
            else
            {
                newFolder = Directory.CreateDirectory(folder + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\main\db\sql");
            }

            writeSqlSupportJava(newFolder.FullName, sqlInfo);
            writeSqlJava(newFolder.FullName, sqlInfo);
        }

        private void writeSqlJava(string newFolder, HcSQLInfo sqlInfo)
        {
            var file = new FileStream(string.Format(@"{0}\{1}DTO.java", newFolder, sqlInfo.Name.ToTitleCase()), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/*");
                writer.WriteLine(@" * {0}", ToolLibrary.Constants.ProjectName);
                writer.WriteLine(@" * {0}", ToolLibrary.Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".main.db.sql;");
                writer.WriteLine(@"");
                writer.WriteLine(@"import " + Constants.Package + @".main.db.sql.support.{0}DTOSupport;", sqlInfo.Name.ToTitleCase());
                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" *Service DB DTO");
                writer.WriteLine(@" *");
                writer.WriteLine(@" * History");
                writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                writer.WriteLine(@" * 1.0          {0}             TOOL                 Create", DateTime.Now.ToString("yyyy/MM/dd"));
                writer.WriteLine(@"*");
                writer.WriteLine(@" */");
                writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.Alias(""{0}"")", sqlInfo.Name.ToUpper());
                writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.AliasKanJi("""")");
                writer.WriteLine(@"public class {0}DTO extends {0}DTOSupport implements java.io.Serializable, Comparable<{0}DTO> {{", sqlInfo.Name.ToTitleCase());
                writer.WriteLine(@"");
                writer.WriteLine(@"	private static final long serialVersionUID = 1L;");

                var i = 0;
                foreach (var column in sqlInfo.Columns)
                {
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Alias(""{0}"")", column.ColumnName.ToUpper());
                    writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Index({0})", i);
                    writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.DataType(""{0}"")", column.DataTypeName);
                    if (column.IsNullAble == false)
                    {
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.NotNull");
                    }
                    if (column.DataType == MySqlDbType.VarChar || column.DataType == MySqlDbType.Blob)
                    {
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Length(min = {0}, max = {0})", column.Character_Maximum_Length);
                    }
                    if (column.ColumnKey == true)
                    {
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.IsPK");
                    }
                    writer.WriteLine(@"	private {0} {1} = null;", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                    i++;
                }

                foreach (var column in sqlInfo.Columns)
                {
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public {0} get{1}() {{", getJavaTypeString(column.DataType), column.ColumnName.ToPublicDefinition());
                    writer.WriteLine(@"		return this.{0};", column.ColumnName.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public void set{0}(", column.ColumnName.ToPublicDefinition());
                    writer.WriteLine(@"			{0} {1}) {{", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                    writer.WriteLine(@"		this.{0} = {0};", column.ColumnName.ToPrivateDefinition());
                    writer.WriteLine(@"	}");
                }
                writer.WriteLine(@"");
                writer.WriteLine(@"  	/* (non-Javadoc)");
                writer.WriteLine(@"     * @see java.lang.Comparable#compareTo(java.lang.Object)");
                writer.WriteLine(@"     */");
                writer.WriteLine(@"    @Override");
                writer.WriteLine(@"    public int compareTo(");
                writer.WriteLine(@"			{0}DTO compareToDTO) {{", sqlInfo.Name.ToTitleCase());
                writer.WriteLine(@"        return this.equals(compareToDTO) ? 0 : -1;");
                writer.WriteLine(@"    }");
                writer.WriteLine(@"}");
                writer.WriteLine(@"");
            }
        }

        private void writeSqlSupportJava(string path, HcSQLInfo sqlInfo)
        {
            var newFolder = Directory.CreateDirectory(path + @"\support");
            var file = File.Create(string.Format(@"{0}\{1}DTOSupport.java", newFolder.FullName, sqlInfo.Name.ToTitleCase()));
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/*");
                writer.WriteLine(@" * {0}", ToolLibrary.Constants.ProjectName);
                writer.WriteLine(@" * {0}", ToolLibrary.Constants.CompanyName);
                writer.WriteLine(@" */");
                writer.WriteLine(@"package " + Constants.Package + @".main.db.sql.support;");
                writer.WriteLine(@"");
                if (sqlInfo.Columns.FindAll(d => d.DataType == MySqlDbType.VarChar).Count > 0)
                {
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.util.StringUtil;");
                }
                if (sqlInfo.Columns.FindAll(d => d.DataType == MySqlDbType.Date || d.DataType == MySqlDbType.Time).Count > 0)
                {
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.util.DateUtil;");
                }
                if (sqlInfo.Columns.FindAll(d => d.DataType == MySqlDbType.Blob).Count > 0)
                {
                    writer.WriteLine(@"import " + Constants.Package + @".fw.core.sql.CISBlob;");
                }
                writer.WriteLine(@"import " + Constants.Package + @".main.db.base.BaseTableDTO;");
                writer.WriteLine(@"import " + Constants.Package + @".main.db.sql.{0}DTO;", sqlInfo.Name.ToTitleCase());
                writer.WriteLine(@"");
                writer.WriteLine(@"/**");
                writer.WriteLine(@" *Service DB Support DTO");
                writer.WriteLine(@" *");
                writer.WriteLine(@" * History");
                writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                writer.WriteLine(@" * 1.0          {0}             TOOL                 Create", DateTime.Now.ToString("yyyy/MM/dd"));
                writer.WriteLine(@"*");
                writer.WriteLine(@" */");
                writer.WriteLine(@"public abstract class {0}DTOSupport extends BaseTableDTO {{", sqlInfo.Name.ToTitleCase());
                writer.WriteLine(@"");

                foreach (var column in sqlInfo.Columns)
                {
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String COLUMN_{0} = ""{0}"";", column.ColumnName.ToUpper());
                }

                writer.WriteLine(@"");
                writer.WriteLine(@"	public static final String SQL_NAME = ""{0}"";", sqlInfo.Key);

                writer.WriteLine(@"");
                writer.WriteLine(@"	public static {0}DTO setDefaultValue() {{", sqlInfo.Name.ToTitleCase());
                writer.WriteLine(@"		{0}DTO defaultDTO = new {0}DTO();", sqlInfo.Name.ToTitleCase());

                foreach (var column in sqlInfo.Columns)
                {
                    writer.WriteLine(@"");
                    writer.WriteLine(@"		defaultDTO.set{0}({1});", column.ColumnName.ToPublicDefinition(), getDefaultValue(column.DataType));
                }

                writer.WriteLine(@"");
                writer.WriteLine(@"		return defaultDTO;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"}");
            }
        }

        private void createViewJava(string folder, List<HcViewInfo> views)
        {
            var newFolder = Directory.CreateDirectory(folder + @"\src\" + Constants.Package.Replace(@".",@"\") + @"\main\db\view");

            writeViewSupportJava(newFolder.FullName, views);
            writeViewJava(newFolder.FullName, views);
        }

        private void writeViewSupportJava(string path, List<HcViewInfo> views)
        {
            var newFolder = Directory.CreateDirectory(path + @"\support");
            foreach (var info in views)
            {
                var file = File.Create(string.Format(@"{0}\{1}DTOSupport.java", newFolder.FullName, info.Name.ToTitleCase()));
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"/*");
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.ProjectName);
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.CompanyName);
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"package " + Constants.Package + @".main.db.view.support;");
                    writer.WriteLine(@"");
                    if (info.Columns.FindAll(d => d.DataType == MySqlDbType.VarChar).Count > 0)
                    {
                        writer.WriteLine(@"import " + Constants.Package + @".fw.core.util.StringUtil;");
                    }
                    if (info.Columns.FindAll(d => d.DataType == MySqlDbType.Date || d.DataType == MySqlDbType.Time).Count > 0)
                    {
                        writer.WriteLine(@"import " + Constants.Package + @".fw.core.util.DateUtil;");
                    }
                    if (info.Columns.FindAll(d => d.DataType == MySqlDbType.Blob).Count > 0)
                    {
                        writer.WriteLine(@"import " + Constants.Package + @".fw.core.sql.CISBlob;");
                    }
                    writer.WriteLine(@"import " + Constants.Package + @".main.db.base.BaseTableDTO;");
                    writer.WriteLine(@"import " + Constants.Package + @".main.db.view.{0}DTO;", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"/**");
                    writer.WriteLine(@" *Service DB Support DTO");
                    writer.WriteLine(@" *");
                    writer.WriteLine(@" * History");
                    writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                    writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                    writer.WriteLine(@" * 1.0          {0}             TOOL                 Create", DateTime.Now.ToString("yyyy/MM/dd"));
                    writer.WriteLine(@"*");
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"public abstract class {0}DTOSupport extends BaseTableDTO {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String EQUAL_SEARCH = ""{0}_EQUAL_SEARCH"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String EQUAL_SEARCH_OR = ""{0}_EQUAL_SEARCH_OR"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String LIKE_SEARCH = ""{0}_LIKE_SEARCH"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String LIKE_SEARCH_OR = ""{0}_LIKE_SEARCH_OR"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String SQL_SEARCH = ""{0}_SQL_SEARCH"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String COUNT = ""{0}_COUNT"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String COUNT_OR = ""{0}_COUNT_OR"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String COUNT_SEARCH = ""{0}_COUNT_SEARCH"";", info.Name.ToUpper());

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public static final String COLUMN_{0} = ""{0}"";", column.ColumnName.ToUpper());
                    }

                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String TABLE = ""{0}"";", info.Name.ToUpper());

                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static {0}DTO setDefaultValue() {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"		{0}DTO defaultDTO = new {0}DTO();", info.Name.ToTitleCase());

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"		defaultDTO.set{0}({1});", column.ColumnName.ToPublicDefinition(), getDefaultValue(column.DataType));
                    }

                    writer.WriteLine(@"");
                    writer.WriteLine(@"		return defaultDTO;");
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"}");
                }
            }
        }

        private void writeViewJava(string newFolder, List<HcViewInfo> views)
        {
            foreach (var info in views)
            {
                var file = new FileStream(string.Format(@"{0}\{1}DTO.java", newFolder, info.Name.ToTitleCase()), FileMode.Create, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"/*");
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.ProjectName);
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.CompanyName);
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"package " + Constants.Package + @".main.db.view;");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"import " + Constants.Package + @".main.db.view.support.{0}DTOSupport;", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"/**");
                    writer.WriteLine(@" *Service DB DTO");
                    writer.WriteLine(@" *");
                    writer.WriteLine(@" * History");
                    writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                    writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                    writer.WriteLine(@" * 1.0          {0}             TOOL                 Create", DateTime.Now.ToString("yyyy/MM/dd"));
                    writer.WriteLine(@"*");
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.Alias(""{0}"")", info.Name.ToUpper());
                    writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.AliasKanJi("""")");
                    writer.WriteLine(@"public class {0}DTO extends {0}DTOSupport implements java.io.Serializable, Comparable<{0}DTO> {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	private static final long serialVersionUID = 1L;");

                    var i = 0;
                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Alias(""{0}"")", column.ColumnName.ToUpper());
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Index({0})", i);
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.DataType(""{0}"")", column.DataTypeName);
                        if (column.IsNullAble == false)
                        {
                            writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.NotNull");
                        }
                        if (column.DataType == MySqlDbType.VarChar || column.DataType == MySqlDbType.Blob)
                        {
                            writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Length(min = {0}, max = {0})", column.Character_Maximum_Length);
                        }
                        if (column.ColumnKey == true)
                        {
                            writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.IsPK");
                        }
                        writer.WriteLine(@"	private {0} {1} = null;", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                        i++;
                    }

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public {0} get{1}() {{", getJavaTypeString(column.DataType), column.ColumnName.ToPublicDefinition());
                        writer.WriteLine(@"		return this.{0};", column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"	}");
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public void set{0}(", column.ColumnName.ToPublicDefinition());
                        writer.WriteLine(@"			{0} {1}) {{", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition().ToPrivateDefinition());
                        writer.WriteLine(@"		this.{0} = {0};", column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"	}");
                    }
                    writer.WriteLine(@"");
                    writer.WriteLine(@"  	/* (non-Javadoc)");
                    writer.WriteLine(@"     * @see java.lang.Comparable#compareTo(java.lang.Object)");
                    writer.WriteLine(@"     */");
                    writer.WriteLine(@"    @Override");
                    writer.WriteLine(@"    public int compareTo(");
                    writer.WriteLine(@"			{0}DTO compareToDTO) {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"        return this.equals(compareToDTO) ? 0 : -1;");
                    writer.WriteLine(@"    }");
                    writer.WriteLine(@"}");
                    writer.WriteLine(@"");
                }
            }
        }

        private void createTblJava(string folder, List<HcTableInfo> tables)
        {
            var newFolder = Directory.CreateDirectory(folder + @"\src\" + Constants.Package.Replace(@".", @"\") + @"\main\db\tbl");

            writeTblSupportJava(newFolder.FullName, tables);

            writeTblJava(newFolder.FullName, tables);
        }

        private void writeTblJava(string newFolder, List<HcTableInfo> tables)
        {
            foreach (var info in tables)
            {
                var file = new FileStream(string.Format(@"{0}\{1}DTO.java", newFolder, info.Name.ToTitleCase()), FileMode.Create, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"/*");
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.ProjectName);
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.CompanyName);
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"package " + Constants.Package + @".main.db.tbl;");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"import " + Constants.Package + @".main.db.tbl.support.{0}DTOSupport;", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"/**");
                    writer.WriteLine(@" *Service DB DTO");
                    writer.WriteLine(@" *");
                    writer.WriteLine(@" * History");
                    writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                    writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                    writer.WriteLine(@" * 1.0          {0}             TOOL                 Create", DateTime.Now.ToString("yyyy/MM/dd"));
                    writer.WriteLine(@"*");
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.Alias(""{0}"")", info.Name.ToUpper());
                    writer.WriteLine(@"@" + Constants.Package + @".fw.core.type.AliasKanJi("""")");
                    writer.WriteLine(@"public class {0}DTO extends {0}DTOSupport implements java.io.Serializable, Comparable<{0}DTO> {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	private static final long serialVersionUID = 1L;");

                    var i = 0;
                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Alias(""{0}"")", column.ColumnName.ToUpper());
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Index({0})", i);
                        writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.DataType(""{0}"")", column.DataTypeName);
                        if (column.IsNullAble == false)
                        {
                            writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.NotNull");
                        }
                        if (column.DataType == MySqlDbType.VarChar || column.DataType == MySqlDbType.Blob)
                        {
                            writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.Length(min = {0}, max = {0})", column.Character_Maximum_Length);
                        }
                        if (column.ColumnKey == true)
                        {
                            writer.WriteLine(@"	@" + Constants.Package + @".fw.core.type.IsPK");
                        }
                        writer.WriteLine(@"	private {0} {1} = null;", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                        i++;
                    }

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public {0} get{1}() {{", getJavaTypeString(column.DataType), column.ColumnName.ToPublicDefinition());
                        writer.WriteLine(@"		return this.{0};", column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"	}");
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public void set{0}(", column.ColumnName.ToPublicDefinition());
                        writer.WriteLine(@"			{0} {1}) {{", getJavaTypeString(column.DataType), column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"		this.{0} = {0};", column.ColumnName.ToPrivateDefinition());
                        writer.WriteLine(@"	}");
                    }
                    writer.WriteLine(@"");
                    writer.WriteLine(@"  	/* (non-Javadoc)");
                    writer.WriteLine(@"     * @see java.lang.Comparable#compareTo(java.lang.Object)");
                    writer.WriteLine(@"     */");
                    writer.WriteLine(@"    @Override");
                    writer.WriteLine(@"    public int compareTo(");
                    writer.WriteLine(@"			{0}DTO compareToDTO) {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"        return this.equals(compareToDTO) ? 0 : -1;");
                    writer.WriteLine(@"    }");
                    writer.WriteLine(@"}");
                    writer.WriteLine(@"");
                }
            }
        }

        private static string getJavaTypeString(MySqlDbType mySqlDbType)
        {
            var ret = string.Empty;
            switch (mySqlDbType)
            {
                case MySqlDbType.VarChar:
                    ret = "String";
                    break;
                case MySqlDbType.Int32:
                    ret = "Integer";
                    break;
                case MySqlDbType.Int64:
                    ret = "Long";
                    break;
                case MySqlDbType.Date:
                    ret = "java.sql.Date";
                    break;
                case MySqlDbType.Blob:
                    ret = "java.sql.Blob";
                    break;
                case MySqlDbType.Time:
                    ret = "java.sql.Time";
                    break;
                case MySqlDbType.Decimal:
                    ret = "java.math.BigDecimal";
                    break;
                case MySqlDbType.Float:
                    ret = "Float";
                    break;
                case MySqlDbType.Double:
                    ret = "Double";
                    break;
                case MySqlDbType.Timestamp:
                    ret = "java.sql.Timestamp";
                    break;
            }
            return ret;
        }

        private string getCshapTypeString(MySqlDbType mySqlDbType)
        {
            var ret = string.Empty;
            switch (mySqlDbType)
            {
                case MySqlDbType.VarChar:
                    ret = "string";
                    break;
                case MySqlDbType.Int32:
                    ret = "int";
                    break;
                case MySqlDbType.Date:
                    ret = "string";
                    break;
                case MySqlDbType.Blob:
                    ret = "string";
                    break;
                case MySqlDbType.Time:
                    ret = "string";
                    break;
            }
            return ret;
        }

        private void writeTblSupportJava(string folder, List<HcTableInfo> tables)
        {
            var newFolder = Directory.CreateDirectory(folder + @"\support");
            foreach (var info in tables)
            {
                var file = new FileStream(string.Format(@"{0}\{1}DTOSupport.java", newFolder.FullName, info.Name.ToTitleCase()), FileMode.Create, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"/*");
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.ProjectName);
                    writer.WriteLine(@" * {0}", ToolLibrary.Constants.CompanyName);
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"package " + Constants.Package + @".main.db.tbl.support;");
                    writer.WriteLine(@"");
                    if (info.Columns.FindAll(d => d.DataType == MySqlDbType.VarChar).Count > 0)
                    {
                        writer.WriteLine(@"import " + Constants.Package + @".fw.core.util.StringUtil;");
                    }
                    if (info.Columns.FindAll(d => d.DataType == MySqlDbType.Date || d.DataType == MySqlDbType.Time).Count > 0)
                    {
                        writer.WriteLine(@"import " + Constants.Package + @".fw.core.util.DateUtil;");
                    }
                    if (info.Columns.FindAll(d => d.DataType == MySqlDbType.Blob).Count > 0)
                    {
                        writer.WriteLine(@"import " + Constants.Package + @".fw.core.sql.CISBlob;");
                    }
                    writer.WriteLine(@"import " + Constants.Package + @".main.db.base.BaseTableDTO;");
                    writer.WriteLine(@"import " + Constants.Package + @".main.db.tbl.{0}DTO;", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"/**");
                    writer.WriteLine(@" *Service DB Support DTO");
                    writer.WriteLine(@" *");
                    writer.WriteLine(@" * History");
                    writer.WriteLine(@" * REV.         Updated Date           Updater              Infomation");
                    writer.WriteLine(@" * -------      ---------------        ----------------     ------------------");
                    writer.WriteLine(@" * 1.0          {0}             TOOL                 Create", DateTime.Now.ToString("yyyy/MM/dd"));
                    writer.WriteLine(@"*");
                    writer.WriteLine(@" */");
                    writer.WriteLine(@"public abstract class {0}DTOSupport extends BaseTableDTO {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String INSERT = ""{0}_INSERT"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String FULL_UPDATE = ""{0}_FULL_UPDATE"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String UPDATE = ""{0}_UPDATE"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String DELETE = ""{0}_DELETE"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String FIND = ""{0}_FIND"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String FIND_FOR_UPDATE = ""{0}_FIND_FOR_UPDATE"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String EQUAL_SEARCH = ""{0}_EQUAL_SEARCH"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String EQUAL_SEARCH_OR = ""{0}_EQUAL_SEARCH_OR"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String LIKE_SEARCH = ""{0}_LIKE_SEARCH"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String LIKE_SEARCH_OR = ""{0}_LIKE_SEARCH_OR"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String SQL_SEARCH = ""{0}_SQL_SEARCH"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String COUNT = ""{0}_COUNT"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String COUNT_OR = ""{0}_COUNT_OR"";", info.Name.ToUpper());
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String COUNT_SEARCH = ""{0}_COUNT_SEARCH"";", info.Name.ToUpper());

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"	public static final String COLUMN_{0} = ""{0}"";", column.ColumnName.ToUpper());
                    }

                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static final String TABLE = ""{0}"";", info.Name.ToUpper());

                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public static {0}DTO setDefaultValue() {{", info.Name.ToTitleCase());
                    writer.WriteLine(@"		{0}DTO defaultDTO = new {0}DTO();", info.Name.ToTitleCase());

                    foreach (var column in info.Columns)
                    {
                        writer.WriteLine(@"");
                        writer.WriteLine(@"		defaultDTO.set{0}({1});", column.ColumnName.ToPublicDefinition(), getDefaultValue(column.DataType));
                    }

                    writer.WriteLine(@"");
                    writer.WriteLine(@"		return defaultDTO;");
                    writer.WriteLine(@"	}");
                    writer.WriteLine(@"}");
                }
            }
        }

        private string getDefaultValue(MySqlDbType mySqlDbType)
        {
            var ret = string.Empty;
            switch (mySqlDbType)
            {
                case MySqlDbType.VarChar:
                    ret = "StringUtil.EMPTY";
                    break;
                case MySqlDbType.Blob:
                    ret = "new CISBlob(new byte[0])";
                    break;
                case MySqlDbType.Int32:
                    ret = "0";
                    break;
                case MySqlDbType.Int64:
                    ret = "0l";
                    break;
                case MySqlDbType.Decimal:
                    ret = "new java.math.BigDecimal(0)";
                    break;
                case MySqlDbType.Date:
                    ret = "DateUtil.getDefaultDate()";
                    break;
                case MySqlDbType.Time:
                    ret = "DateUtil.getDefaultTime()";
                    break;
                case MySqlDbType.Float:
                    ret = "0f";
                    break;
                case MySqlDbType.Double:
                    ret = "0.0";
                    break;
                case MySqlDbType.DateTime:
                    ret = "DateUtil.getDefaultDateTime()";
                    break;
                case MySqlDbType.Timestamp:
                    ret = "new java.sql.Timestamp(System.currentTimeMillis())";
                    break;
                default:
                    throw new Exception();
            }
            return ret;
        }

        private void createMyBatis(string folder, List<HcTableInfo> tables, List<HcViewInfo> views)
        {
            createMybatisConfig(folder, tables, views);

            createMybatisTbl(folder, tables);

            createMybatisView(folder, views);
        }

        private void createMybatisTbl(string folder, List<HcTableInfo> tables)
        {
            var newFolder = Directory.CreateDirectory(folder + @"\src\sql\tbl");

            foreach (var info in tables)
            {
                var file = new FileStream(string.Format(@"{0}\{1}.xml", newFolder.FullName, info.Name.ToUpper()), FileMode.Create, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>");
                    writer.WriteLine(@"<!DOCTYPE mapper PUBLIC ""-//mybatis.org//DTD Mapper 3.0//EN"" ""http://mybatis.org/dtd/mybatis-3-mapper.dtd"">");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"<mapper namespace=""sql"">");
                    writer.WriteLine(@"");
                    writeInsert(info, writer);
                    writer.WriteLine(@"");
                    writeFullUpdate(info, writer);
                    writer.WriteLine(@"");
                    writeUpdate(info, writer);
                    writer.WriteLine(@"");
                    writeDelete(info, writer);
                    writer.WriteLine(@"");
                    writeFind(info, writer);
                    writer.WriteLine(@"");
                    writeFindForUpdate(info, writer);
                    writer.WriteLine(@"");
                    writeEqualSearch(info, writer);
                    writer.WriteLine(@"");
                    writeEqualSearchOR(info, writer);
                    writer.WriteLine(@"");
                    writeLikeSearch(info, writer);
                    writer.WriteLine(@"");
                    writeLikeSearchOr(info, writer);
                    writer.WriteLine(@"");
                    writeSQLSearch(info, writer);
                    writer.WriteLine(@"");
                    writeCount(info, writer);
                    writer.WriteLine(@"");
                    writeCountOR(info, writer);
                    writer.WriteLine(@"");
                    writeCountSearch(info, writer);
                    writer.WriteLine(@"</mapper>");
                }
            }
        }

        private void createMybatisView(string folder, List<HcViewInfo> views)
        {
            var newFolder = Directory.CreateDirectory(folder + @"\src\sql\view");

            foreach (var info in views)
            {
                var file = new FileStream(string.Format(@"{0}\{1}.xml", newFolder.FullName, info.Name.ToUpper()), FileMode.Create, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>");
                    writer.WriteLine(@"<!DOCTYPE mapper PUBLIC ""-//mybatis.org//DTD Mapper 3.0//EN"" ""http://mybatis.org/dtd/mybatis-3-mapper.dtd"">");
                    writer.WriteLine(@"");
                    writer.WriteLine(@"<mapper namespace=""sql"">");
                    writer.WriteLine(@"");
                    writeEqualSearch(info, writer);
                    writer.WriteLine(@"");
                    writeEqualSearchOR(info, writer);
                    writer.WriteLine(@"");
                    writeLikeSearch(info, writer);
                    writer.WriteLine(@"");
                    writeLikeSearchOr(info, writer);
                    writer.WriteLine(@"");
                    writeSQLSearch(info, writer);
                    writer.WriteLine(@"");
                    writeCount(info, writer);
                    writer.WriteLine(@"");
                    writeCountOR(info, writer);
                    writer.WriteLine(@"");
                    writeCountSearch(info, writer);
                    writer.WriteLine(@"</mapper>");
                }
            }
        }

        private static void writeCountSearch(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Count Customize -->");
            writer.WriteLine(@"  <select id=""{0}_COUNT_SEARCH"" resultType=""java.lang.Integer"" parameterType=""java.util.Map"">", info.Name.ToUpper());
            writer.WriteLine(@"    SELECT count(*)");
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <if test=""_dynamicWhereCondition != null"">WHERE ${_dynamicWhereCondition}</if>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeCountOR(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Count OR -->");
            writer.WriteLine(@"  <select id=""{0}_COUNT_OR"" resultType=""java.lang.Integer"">", info.Name.ToUpper());
            writer.WriteLine(@"    SELECT count(*)");
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <where>");
            foreach (var column in info.Columns)
            {
                writer.WriteLine(@"      <if test=""{0} != null"">OR {1} = #{{{2}}}</if>",
                    column.ColumnName.ToPrivateDefinition(), column.ColumnName.ToUpper(),
                    column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(@"    </where>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeCount(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Count -->");
            writer.WriteLine(@"  <select id=""{0}_COUNT"" resultType=""java.lang.Integer"">", info.Name.ToUpper());
            writer.WriteLine(@"    SELECT count(*)");
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <where>");
            foreach (var column in info.Columns)
            {
                writer.WriteLine(@"      <if test=""{0} != null"">AND {1} = #{{{2}}}</if>",
                    column.ColumnName.ToPrivateDefinition(), column.ColumnName.ToUpper(),
                    column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(@"    </where>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeSQLSearch(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Customize Search -->");
            if (info.GetType() == typeof(HcTableInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_SQL_SEARCH"" resultType=""" + Constants.Package + @".main.db.tbl.{1}DTO"" parameterType=""java.util.Map"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }
            else if (info.GetType() == typeof(HcViewInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_SQL_SEARCH"" resultType=""" + Constants.Package + @".main.db.view.{1}DTO"" parameterType=""java.util.Map"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }

            writer.WriteLine(@"    SELECT");

            var writeLine = string.Empty;
            foreach (var column in info.Columns)
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                    writeLine = string.Empty;
                }

                writeLine = string.Format(@"      {0} AS ""{1}"" ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(writeLine);
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <if test=""_dynamicWhereCondition != null"">WHERE ${_dynamicWhereCondition}</if>");

            writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.ColumnKey == true))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writeLine = string.Format("{0},", writeLine);
                }
                writeLine = string.Format(@"{0} {1}", writeLine, column.ColumnName.ToUpper());
            }
            if (!string.IsNullOrEmpty(writeLine))
            {
                writer.WriteLine(@"    <if test=""_orderByCondition == null"">ORDER BY{0}</if>", writeLine);
            }
            writer.WriteLine(@"    <if test=""_orderByCondition != null"">ORDER BY ${_orderByCondition}</if>");
            writer.WriteLine(@"    <if test=""_fetchFirstCondition != null"">FETCH FIRST ${_fetchFirstCondition} ROWS ONLY</if>");
            writer.WriteLine(@"    <if test=""_limitCondition != null"">LIMIT ${_limitCondition}</if>");
            writer.WriteLine(@"    <if test=""_offsetCondition != null"">OFFSET ${_offsetCondition}</if>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeLikeSearchOr(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Search LIKE(OR) -->");
            if (info.GetType() == typeof(HcTableInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_LIKE_SEARCH_OR"" resultType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }
            else if (info.GetType() == typeof(HcViewInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_LIKE_SEARCH_OR"" resultType=""" + Constants.Package + @".main.db.view.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }

            writer.WriteLine(@"    SELECT");

            var writeLine = string.Empty;
            foreach (var column in info.Columns)
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                    writeLine = string.Empty;
                }

                writeLine = string.Format(@"      {0} AS ""{1}"" ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(writeLine);
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <where>");
            foreach (var column in info.Columns)
            {
                writer.WriteLine(@"      <if test=""{0} != null"">OR {1} LIKE '%'|| #{{{2}}} ||'%'</if>",
                    column.ColumnName.ToPrivateDefinition(), column.ColumnName.ToUpper(),
                    column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(@"    </where>");
            writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.ColumnKey == true))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writeLine = string.Format("{0},", writeLine);
                }
                writeLine = string.Format(@"{0} {1}", writeLine, column.ColumnName.ToUpper());
            }
            if (!string.IsNullOrEmpty(writeLine))
            {
                writer.WriteLine(@"    <if test=""_orderByCondition == null"">ORDER BY{0}</if>", writeLine);
            }

            writer.WriteLine(@"    <if test=""_orderByCondition != null"">ORDER BY ${_orderByCondition}</if>");
            writer.WriteLine(@"    <if test=""_fetchFirstCondition != null"">FETCH FIRST ${_fetchFirstCondition} ROWS ONLY</if>");
            writer.WriteLine(@"    <if test=""_limitCondition != null"">LIMIT ${_limitCondition}</if>");
            writer.WriteLine(@"    <if test=""_offsetCondition != null"">OFFSET ${_offsetCondition}</if>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeLikeSearch(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Search LIKE -->");
            if (info.GetType() == typeof(HcTableInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_LIKE_SEARCH"" resultType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }
            else if (info.GetType() == typeof(HcViewInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_LIKE_SEARCH"" resultType=""" + Constants.Package + @".main.db.view.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }

            writer.WriteLine(@"    SELECT");

            var writeLine = string.Empty;
            foreach (var column in info.Columns)
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                    writeLine = string.Empty;
                }

                writeLine = string.Format(@"      {0} AS ""{1}"" ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(writeLine);
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <where>");
            foreach (var column in info.Columns)
            {
                writer.WriteLine(@"      <if test=""{0} != null"">AND {1} LIKE '%'|| #{{{2}}} ||'%'</if>",
                    column.ColumnName.ToPrivateDefinition(), column.ColumnName.ToUpper(),
                    column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(@"    </where>");
            writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.ColumnKey == true))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writeLine = string.Format("{0},", writeLine);
                }
                writeLine = string.Format(@"{0} {1}", writeLine, column.ColumnName.ToUpper());
            }
            if (!string.IsNullOrEmpty(writeLine))
            {
                writer.WriteLine(@"    <if test=""_orderByCondition == null"">ORDER BY{0}</if>", writeLine);
            }

            writer.WriteLine(@"    <if test=""_orderByCondition != null"">ORDER BY ${_orderByCondition}</if>");
            writer.WriteLine(@"    <if test=""_fetchFirstCondition != null"">FETCH FIRST ${_fetchFirstCondition} ROWS ONLY</if>");
            writer.WriteLine(@"    <if test=""_limitCondition != null"">LIMIT ${_limitCondition}</if>");
            writer.WriteLine(@"    <if test=""_offsetCondition != null"">OFFSET ${_offsetCondition}</if>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeEqualSearchOR(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Search(OR) -->");
            if (info.GetType() == typeof(HcTableInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_EQUAL_SEARCH_OR"" resultType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }
            else if (info.GetType() == typeof(HcViewInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_EQUAL_SEARCH_OR"" resultType=""" + Constants.Package + @".main.db.view.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }

            writer.WriteLine(@"    SELECT");

            var writeLine = string.Empty;
            foreach (var column in info.Columns)
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                    writeLine = string.Empty;
                }

                writeLine = string.Format(@"      {0} AS ""{1}"" ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(writeLine);
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <where>");
            foreach (var column in info.Columns)
            {
                writer.WriteLine(@"      <if test=""{0} != null"">OR {1} = #{{{2}}}</if>",
                    column.ColumnName.ToPrivateDefinition(), column.ColumnName.ToUpper(),
                    column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(@"    </where>");
            writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.ColumnKey == true))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writeLine = string.Format("{0},", writeLine);
                }
                writeLine = string.Format(@"{0} {1}", writeLine, column.ColumnName.ToUpper());
            }
            if (!string.IsNullOrEmpty(writeLine))
            {
                writer.WriteLine(@"    <if test=""_orderByCondition == null"">ORDER BY{0}</if>", writeLine);
            }

            writer.WriteLine(@"    <if test=""_orderByCondition != null"">ORDER BY ${_orderByCondition}</if>");
            writer.WriteLine(@"    <if test=""_fetchFirstCondition != null"">FETCH FIRST ${_fetchFirstCondition} ROWS ONLY</if>");
            writer.WriteLine(@"    <if test=""_limitCondition != null"">LIMIT ${_limitCondition}</if>");
            writer.WriteLine(@"    <if test=""_offsetCondition != null"">OFFSET ${_offsetCondition}</if>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeEqualSearch(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- AND Search -->");
            if (info.GetType() == typeof(HcTableInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_EQUAL_SEARCH"" resultType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }
            else if (info.GetType() == typeof(HcViewInfo))
            {
                writer.WriteLine(@"  <select id=""{0}_EQUAL_SEARCH"" resultType=""" + Constants.Package + @".main.db.view.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            }

            writer.WriteLine(@"    SELECT");
            var writeLine = string.Empty;
            foreach (var column in info.Columns)
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                    writeLine = string.Empty;
                }

                writeLine = string.Format(@"      {0} AS ""{1}"" ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(writeLine);
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <where>");
            foreach (var column in info.Columns)
            {
                writer.WriteLine(@"      <if test=""{0} != null"">AND {1} = #{{{2}}}</if>", column.ColumnName.ToPrivateDefinition(), column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(@"    </where>");
            writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.ColumnKey == true))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writeLine = string.Format("{0},", writeLine);
                }
                writeLine = string.Format(@"{0} {1}", writeLine, column.ColumnName.ToUpper());
            }
            if (!string.IsNullOrEmpty(writeLine))
            {
                writer.WriteLine(@"    <if test=""_orderByCondition == null"">ORDER BY{0}</if>", writeLine);
            }

            writer.WriteLine(@"    <if test=""_orderByCondition != null"">ORDER BY ${_orderByCondition}</if>");
            writer.WriteLine(@"    <if test=""_fetchFirstCondition != null"">FETCH FIRST ${_fetchFirstCondition} ROWS ONLY</if>");
            writer.WriteLine(@"    <if test=""_limitCondition != null"">LIMIT ${_limitCondition}</if>");
            writer.WriteLine(@"    <if test=""_offsetCondition != null"">OFFSET ${_offsetCondition}</if>");
            writer.WriteLine(@"  </select>");
        }

        private static void writeFindForUpdate(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Key Search（For Update） -->");
            writer.WriteLine(@"  <select id=""{0}_FIND_FOR_UPDATE"" resultType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            writer.WriteLine(@"    SELECT");
            var writeLine = string.Empty;
            foreach (var column in info.Columns)
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                    writeLine = string.Empty;
                }

                writeLine = string.Format(@"      {0} AS ""{1}"" ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(writeLine);
            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());

            var lst = info.Columns.FindAll(d => d.ColumnKey == true);
            if (lst.Count > 0)
            {
                writer.WriteLine(@"    WHERE");
                writeLine = string.Empty;
                foreach (var column in lst)
                {
                    if (!string.IsNullOrEmpty(writeLine))
                    {
                        writer.WriteLine("{0}AND", writeLine);
                    }
                    writeLine = string.Format(@"      {0} = #{{{1}}} ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
                }
                writer.WriteLine(writeLine);
                writer.WriteLine(@"    FOR UPDATE");
            }
            writer.WriteLine(@"  </select>");
        }

        private static void writeFind(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Key Search -->");
            writer.WriteLine(@"  <select id=""{0}_FIND"" resultType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            writer.WriteLine(@"    SELECT");

            var writeLine = string.Empty;
            foreach (var column in info.Columns)
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                }
                writeLine = string.Format(@"      {0} AS ""{1}"" ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
            }
            writer.WriteLine(writeLine);

            writer.WriteLine(@"    FROM {0}", info.Name.ToUpper());

            var lst = info.Columns.FindAll(d => d.ColumnKey == true);
            if (lst.Count > 0)
            {
                writer.WriteLine(@"    WHERE");

                writeLine = string.Empty;
                foreach (var column in lst)
                {
                    if (!string.IsNullOrEmpty(writeLine))
                    {
                        writer.WriteLine("{0}AND", writeLine);
                    }
                    writeLine = string.Format(@"      {0} = #{{{1}}} ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
                }
                writer.WriteLine(writeLine);
            }
            writer.WriteLine(@"  </select>");
        }

        private static void writeDelete(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Key Delete -->");
            writer.WriteLine(@"  <delete id=""{0}_DELETE"" parameterType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            writer.WriteLine(@"    DELETE FROM {0}", info.Name.ToUpper());

            var lst = info.Columns.FindAll(d => d.ColumnKey == true);
            if (lst.Count > 0)
            {
                writer.WriteLine(@"    WHERE");

                var writeLine = string.Empty;
                foreach (var column in lst)
                {
                    if (!string.IsNullOrEmpty(writeLine))
                    {
                        writer.WriteLine("{0}AND", writeLine);
                    }
                    writeLine = string.Format(@"      {0} = #{{{1}}} ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
                }
                writer.WriteLine(writeLine);
            }
            writer.WriteLine(@"  </delete>");
        }

        private static void writeUpdate(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <update id=""{0}_UPDATE"" parameterType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            writer.WriteLine(@"    UPDATE {0}", info.Name.ToUpper());
            writer.WriteLine(@"    <set>");

            foreach (var column in info.Columns.FindAll(d => d.ColumnKey == false))
            {
                writer.WriteLine(@"      <if test=""{0}!=null"">{1} = {2},</if>",
                    column.ColumnName.ToPrivateDefinition(), column.ColumnName.ToUpper(),
                    getUpdateDataTime(column.ColumnName.ToPrivateDefinition()));
            }

            writer.WriteLine(@"    </set>");

            var lst = info.Columns.FindAll(d => d.ColumnKey == true);
            if (lst.Count > 0)
            {
                writer.WriteLine(@"    WHERE");

                var writeLine = string.Empty;
                foreach (var column in lst)
                {
                    if (!string.IsNullOrEmpty(writeLine))
                    {
                        writer.WriteLine("{0}AND", writeLine);
                    }
                    writeLine = string.Format(@"      {0} = #{{{1}}} ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
                }
                writer.WriteLine(writeLine);
            }
            writer.WriteLine(@"  </update>");
        }

        private static void writeFullUpdate(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Table Update -->");
            writer.WriteLine(@"  <update id=""{0}_FULL_UPDATE"" parameterType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            writer.WriteLine(@"    UPDATE {0} SET", info.Name.ToUpper());

            var writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.ColumnKey == false))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                    writeLine = string.Empty;
                }

                writeLine = string.Format(@"      {0} = {1} ", column.ColumnName.ToUpper(), getUpdateDataTime(column.ColumnName.ToPrivateDefinition()));
            }
            writer.WriteLine(writeLine);

            var lst = info.Columns.FindAll(d => d.ColumnKey == true);
            if (lst.Count > 0)
            {
                writer.WriteLine(@"    WHERE");

                writeLine = string.Empty;
                foreach (var column in lst)
                {
                    if (!string.IsNullOrEmpty(writeLine))
                    {
                        writer.WriteLine("{0}AND", writeLine);
                    }
                    writeLine = string.Format(@"      {0} = #{{{1}}} ", column.ColumnName.ToUpper(), column.ColumnName.ToPrivateDefinition());
                }
                writer.WriteLine(writeLine);
            }

            writer.WriteLine(@"  </update>");
        }

        private static void writeInsert(HcDBBaseInfo info, StreamWriter writer)
        {
            writer.WriteLine(@"  <!-- Table Insert -->");
            writer.WriteLine(@"  <insert id=""{0}_INSERT"" parameterType=""" + Constants.Package + @".main.db.tbl.{1}DTO"">", info.Name.ToUpper(), info.Name.ToTitleCase());
            writer.WriteLine(@"    INSERT INTO {0} (", info.Name.ToUpper());

            var writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.AutoIncrement == false))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                }
                writeLine = string.Format(@"      {0} ", column.ColumnName.ToUpper());
            }
            writer.WriteLine(writeLine);

            writer.WriteLine(@"    ) VALUES (");

            writeLine = string.Empty;
            foreach (var column in info.Columns.FindAll(d => d.AutoIncrement == false))
            {
                if (!string.IsNullOrEmpty(writeLine))
                {
                    writer.WriteLine("{0},", writeLine);
                }
                writeLine = string.Format(@"      {0} ", getUpdateDataTime(column.ColumnName.ToPrivateDefinition()));
            }
            writer.WriteLine(writeLine);

            writer.WriteLine(@"    )");

            var keys = info.Columns.FindAll(d => d.ColumnKey == true && d.AutoIncrement == true);
            if (keys.Count > 0)
            {
                writer.WriteLine(@"    <selectKey resultType=""java.lang.{0}"" order=""AFTER"" keyProperty = ""{1}"">", getJavaTypeString(keys[0].DataType), keys[0].ColumnName.ToPrivateDefinition());
                writer.WriteLine(@"    SELECT LAST_INSERT_ID() AS ID");
                writer.WriteLine(@"    </selectKey>");
            }
            writer.WriteLine(@"  </insert>");
        }

        public static string getUpdateDataTime(string value)
        {
            var ret = string.Empty;
            switch (value.ToLower())
            {
                case "lastdate":
                    ret = "curdate()";
                    break;
                case "lasttime":
                    ret = "curtime()";
                    break;
                default:
                    ret = string.Format(@"#{{{0}}}", value);
                    break;
            }
            return ret;
        }

        private void createMybatisConfig(string folder, List<HcTableInfo> tables, List<HcViewInfo> views)
        {
            var dir = Directory.CreateDirectory(folder + @"\src");
            var file = File.Create(dir.FullName + @"\mybatis-config.xml");
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>");
                writer.WriteLine(@"<!DOCTYPE configuration PUBLIC ""-//mybatis.org//DTD Config 3.0//EN"" ""http://mybatis.org/dtd/mybatis-3-config.dtd"">");
                writer.WriteLine(@"");
                writer.WriteLine(@"<configuration>");
                writer.WriteLine(@"	<settings>");
                writer.WriteLine(@"		<setting name=""cacheEnabled"" value=""false""/>");
                writer.WriteLine(@"	</settings>");
                writer.WriteLine(@"	<typeHandlers>");
                writer.WriteLine(@"		<typeHandler javaType=""java.sql.Blob"" handler=""" + Constants.Package + @".fw.core.dao.lob.ex.CISBlobTypeHandler""/>");
                writer.WriteLine(@"		<typeHandler javaType=""java.sql.Clob"" handler=""" + Constants.Package + @".fw.core.dao.lob.ex.CISClobTypeHandler""/>");
                writer.WriteLine(@"	</typeHandlers>");
                writer.WriteLine(@"	<environments default=""product"">");
                writer.WriteLine(@"		<environment id=""product"">");
                writer.WriteLine(@"			<transactionManager type=""JDBC"" />");
                writer.WriteLine(@"			<dataSource type=""JNDI"">");
                writer.WriteLine(@"				<property name=""data_source"" value=""java:comp/env/jdbc/CISDataSource"" />");
                writer.WriteLine(@"			</dataSource>");
                writer.WriteLine(@"		</environment>");
                writer.WriteLine(@"");
                writer.WriteLine(@"		<environment id=""refer"">");
                writer.WriteLine(@"			<transactionManager type=""JDBC"" />");
                writer.WriteLine(@"			<dataSource type=""JNDI"">");
                writer.WriteLine(@"				<property name=""data_source"" value=""java:comp/env/jdbc/CISDataSourceRefer"" />");
                writer.WriteLine(@"			</dataSource>");
                writer.WriteLine(@"		</environment>");
                writer.WriteLine(@"	</environments>");
                writer.WriteLine(@"");
                writer.WriteLine(@"	<mappers>");
                writer.WriteLine(@"		<!-- Common -->");
                writer.WriteLine(@"		<mapper resource=""sql/cmt/comm.DBConnectionCheck.xml"" />");
                writer.WriteLine(@"		<!-- Customize -->");
                writer.WriteLine(@"		<mapper resource=""sql/cmt/custom.xml"" />");
                writer.WriteLine(@"		<!-- Automatic generation -->");

                foreach (var info in tables)
                {
                    writer.WriteLine(string.Format(@"		<mapper resource=""sql/tbl/{0}.xml"" />", info.Name.ToUpper()));
                }

                foreach (var info in views)
                {
                    writer.WriteLine(string.Format(@"		<mapper resource=""sql/view/{0}.xml"" />", info.Name.ToUpper()));
                }
                writer.WriteLine(@"	</mappers>");
                writer.WriteLine(@"</configuration>");
            }
        }
    }
}
