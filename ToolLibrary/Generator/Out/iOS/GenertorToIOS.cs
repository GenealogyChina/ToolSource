using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolLibrary.Model.Core;
using ToolLibrary.Type;
using Utility.Extension;

namespace ToolLibrary.Generator.Out.iOS
{
    public class GenertorToIOS : IFExportSource
    {
        public void WriteOutPut(String path, HcSubInfo info)
        {
            path = path + @"\IOS\" + info.Name;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            var newFolder = Directory.CreateDirectory(path);
            foreach (var service in info.ServiceArray)
            {
                createModel(newFolder, service);
            }

            if (info.DTOArray != null)
            {
                foreach (var dto in info.DTOArray)
                {
                    writeDTO(newFolder, dto);
                }
            }
        }

        private void createModel(DirectoryInfo newFolder, HcServiceInfo service)
        {
            var fileName = service.Name.Remove(0, Constants.ServiceFirstName.Length);

            createModelH(newFolder, service, fileName);

            createModelM(newFolder, service, fileName);
        }

        private void createModelM(DirectoryInfo newFolder, HcServiceInfo service, string fileName)
        {
            var file = new FileStream(string.Format(@"{0}\{1}.m", newFolder.FullName, fileName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  {0}.m", fileName);
                writer.WriteLine(@"//  {0}", Constants.ProjectName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  Created by xiao huama on {0}.", DateTime.Now.ToString("yy/MM/dd"));
                writer.WriteLine(@"//  {0}", Constants.CompanyName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"");
                writer.WriteLine(@"#import ""{0}.h""", fileName);
                writer.WriteLine(@"");
                writer.WriteLine(@"@interface {0}()", fileName);
                foreach (var field in service.InDTO.FieldArray)
                {
                    writer.WriteLine(@"@property(nonatomic,strong){0} *{1}; /**< {2}*/", getFieldTypeInString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition(), field.caption);
                }
                writer.WriteLine(@"@end");
                writer.WriteLine(@"@implementation {0}", fileName);
                writer.WriteLine(@"");
                writer.WriteLine(@"- (instancetype)init");
                writer.WriteLine(@"{");
                writer.WriteLine(@"    self = [super init];");
                writer.WriteLine(@"    if (self) {");
                writer.WriteLine(@"        serviceMethod = @""{0}"";", service.Name);
                writer.WriteLine(@"        self.className = @""{0}"";", fileName);
                writer.WriteLine(@"    }");
                writer.WriteLine(@"    return self;");
                writer.WriteLine(@"}");
                writer.WriteLine(@"");

                var inputParam = new StringBuilder();
                service.InDTO.FieldArray.ForEach(item =>
                {
                    if (inputParam.Length > 0)
                        inputParam.Append(string.Format(" {0}:", item.name.ToPrivateDefinition()));
                    inputParam.Append(string.Format("({0} *){1}", getFieldTypeInString(item.FieldType, item.FieldTypeString), item.name.ToPrivateDefinition()));
                });

                if (inputParam.Length > 0)
                {
                    inputParam.Insert(0, ":");
                }

                writer.WriteLine(@"- (void)postData{0}{{", inputParam.ToString());
                service.InDTO.FieldArray.ForEach(item =>
                {
                    writer.WriteLine(@"    self.{0} = {0};", item.name.ToPrivateDefinition());
                });
                writer.WriteLine(@"    [self requestWebService:^{");
                writer.WriteLine(@"        if (delegate) {");
                writer.WriteLine(@"            [delegate successMethd:self];");
                writer.WriteLine(@"        }");
                writer.WriteLine(@"    } faild:^{");
                writer.WriteLine(@"        if (delegate) {");
                writer.WriteLine(@"            [delegate errorMethd];");
                writer.WriteLine(@"        }");
                writer.WriteLine(@"    }];");
                writer.WriteLine(@"}");
                writer.WriteLine(@"");

                writer.WriteLine(@"-(NSMutableDictionary *)buildRequestData");
                writer.WriteLine(@"{");
                var lstField = service.InDTO.FieldArray.FindAll(d => isBaseType(d.FieldType));
                writer.WriteLine(@"    NSMutableDictionary *dic = [NSMutableDictionary dictionary];");
                if (lstField != null)
                {
                    foreach (var item in lstField)
                    {
                        writer.WriteLine(@"    if (self.{0}) {{", item.name.ToPrivateDefinition());
                        writer.WriteLine(@"        dic[@""{0}""] = self.{0};", item.name.ToPrivateDefinition());
                        writer.WriteLine(@"    }");
                    }
                }

                lstField = service.InDTO.FieldArray.FindAll(d => isArrayType(d.FieldType));
                if (lstField != null)
                {
                    if (lstField.Count > 0)
                    {
                        foreach (var item in lstField)
                        {
                            if (item.FieldType == EmFieldType.DTOArray)
                            {
                                writer.WriteLine(@"    NSMutableArray *array{0} =[NSMutableArray array];", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    for (BaseModel *model in self.{0}) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"        [array{0} addObject:[model buildRequestData]];", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    }");
                                writer.WriteLine(@"    if (array{0}.count>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"        dic[@""{0}""] = array{0};", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    }");
                            }
                            else
                            {
                                writer.WriteLine(@"    if (self.{0}.count>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"        dic[@""{0}""] = self.{0};", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    }");
                            }
                        }
                    }
                }

                lstField = service.InDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO);
                if (lstField != null)
                {
                    if (lstField.Count > 0)
                    {
                        foreach (var item in lstField)
                        {
                            writer.WriteLine(@"    NSDictionary *{0}Dic = [self.{0} buildRequestData];", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"    if ({0}Dic) {{", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"        dic[@""{0}""] = {0}Dic;", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"    }");
                        }
                    }
                }

                writer.WriteLine(@"    return dic;");
                writer.WriteLine(@"}");
                writer.WriteLine(@"");
                writer.WriteLine(@"- (void)loadDataWithJsonData:(id)jsonData");
                writer.WriteLine(@"{");
                writer.WriteLine(@"    if (![jsonData isKindOfClass:[NSDictionary class]]) {");
                writer.WriteLine(@"        return;");
                writer.WriteLine(@"    }");
                writer.WriteLine(@"    if (!jsonData) {");
                writer.WriteLine(@"        return;");
                writer.WriteLine(@"    }");
                foreach (var field in service.OutDTO.FieldArray)
                {
                    if (isNumberType(field.FieldType))
                    {
                        writer.WriteLine(@"    if (![jsonData[@""{0}""] isKindOfClass:[NSNull class]]) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        self.{0} = jsonData[@""{0}""];", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"    }");
                    }
                    else if (field.FieldType == EmFieldType.DTO)
                    {
                        writer.WriteLine(@"    if ([jsonData[@""{0}""] isKindOfClass:[NSDictionary class]]) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        self.{0} = [[{1} alloc] init];", field.name.ToPrivateDefinition(), field.FieldTypeString);
                        writer.WriteLine(@"        [self.{0} loadDataWithJsonData:jsonData[@""{0}""]];", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"    }");
                    }
                    else
                    {
                        writer.WriteLine(@"    if ([jsonData[@""{0}""] isKindOfClass:[NSArray class]]) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        if ([jsonData[@""{0}""] count]>0) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"            NSMutableArray *tempArray = [NSMutableArray array];");
                        writer.WriteLine(@"            for (id data in jsonData[@""{0}""]) {{", field.name.ToPrivateDefinition());
                        if (isStringArray(field.FieldTypeString.Replace("[]", "")))
                        {
                            writer.WriteLine(@"                [tempArray addObject:data];");
                        }
                        else
                        {
                            writer.WriteLine(@"                [tempArray addObject:[[{0} alloc]initWithJsonData:data]];", field.FieldTypeString.Replace("[]", ""));
                        }
                        writer.WriteLine(@"            }");
                        writer.WriteLine(@"            self.{0} = tempArray;", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        }");
                        writer.WriteLine(@"    }");
                    }
                }
                writer.WriteLine(@"}");
                writer.WriteLine(@"");
                writer.WriteLine(@"-(BOOL)checkRequired");
                writer.WriteLine(@"{");
                writer.WriteLine(@"    NSMutableArray *muArray = [[NSMutableArray alloc] init];");
                writer.WriteLine(@"    BOOL rest = YES;");
                foreach (var item in service.InDTO.FieldArray)
                {
                    if (item.FieldCheckType != null && item.FieldCheckType.Count > 0)
                    {
                        foreach (var chk in item.FieldCheckType)
                        {
                            switch (chk)
                            {
                                case EmCheckType.MustEnter:
                                    getMustInput(writer, item);
                                    break;
                                default:
                                    getMustInput(writer, item);
                                    break;
                            }
                        }
                    }
                }
                writer.WriteLine(@"    ");
                writer.WriteLine(@"    if (!rest){");
                writer.WriteLine(@"        self.exceptions = muArray;");
                writer.WriteLine(@"    }");
                writer.WriteLine(@"    return rest;");
                writer.WriteLine(@"}");
                writer.WriteLine(@"");
                writer.WriteLine(@"@end");
            }
        }

        private void createModelH(DirectoryInfo newFolder, HcServiceInfo service, string fileName)
        {
            var file = new FileStream(string.Format(@"{0}\{1}.h", newFolder.FullName, fileName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  {0}.h", fileName);
                writer.WriteLine(@"//  {0}", Constants.ProjectName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  Created by xiao huama on {0}.", DateTime.Now.ToString("yy/MM/dd"));
                writer.WriteLine(@"//  {0}", Constants.CompanyName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"#import ""ServiceBaseModel.h""");

                service.InDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO || d.FieldType == EmFieldType.DTOArray).ForEach(item =>
                {
                    if (item.FieldType == EmFieldType.DTO)
                    {
                        writer.WriteLine(@"#import ""{0}.h""", item.FieldTypeString);
                    }
                    else
                    {
                        writer.WriteLine(@"#import ""{0}.h""", item.FieldTypeString.Replace("[]", String.Empty));
                    }
                });

                service.OutDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO || d.FieldType == EmFieldType.DTOArray).ForEach(item =>
                {
                    if (item.FieldType == EmFieldType.DTO)
                    {
                        writer.WriteLine(@"#import ""{0}.h""", item.FieldTypeString);
                    }
                    else
                    {
                        writer.WriteLine(@"#import ""{0}.h""", item.FieldTypeString.Replace("[]", String.Empty));
                    }
                });

                writer.WriteLine(@"");
                writer.WriteLine(@"@interface {0} : ServiceBaseModel", fileName);
                writer.WriteLine(@"");

                service.OutDTO.FieldArray.ForEach(item =>
                {
                    writer.WriteLine(@"@property(nonatomic,strong){0} *{1}; /**< {2}*/",
                        getFieldTypeOutString(item.FieldType, item.FieldTypeString),
                        item.name.ToPrivateDefinition(),
                        item.caption);
                });

                writer.WriteLine(@"");
                var inputParam = new StringBuilder();
                service.InDTO.FieldArray.ForEach(item =>
                {
                    if (inputParam.Length > 0)
                        inputParam.Append(string.Format(" {0}:", item.name.ToPrivateDefinition()));
                    inputParam.Append(string.Format("({0} *){1}", getFieldTypeInString(item.FieldType, item.FieldTypeString), item.name.ToPrivateDefinition()));
                });

                if (inputParam.Length > 0)
                {
                    inputParam.Insert(0, ":");
                }

                writer.WriteLine(@"- (void)postData{0};", inputParam.ToString());

                writer.WriteLine(@"");
                writer.WriteLine(@"@end");
            }
        }

        private void writeDTO(DirectoryInfo newFolder, HcDTOInfo info)
        {
            var path = newFolder.FullName + @"\DTO";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = string.Format(@"{0}", info.Name);
            var file = new FileStream(string.Format(@"{0}\{1}.h", path, fileName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  {0}.h", fileName);
                writer.WriteLine(@"//  {0}", Constants.ProjectName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  Created by xiao huama on {0}.", DateTime.Now.ToString("yy/MM/dd"));
                writer.WriteLine(@"//  {0}", Constants.CompanyName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"#import ""ServiceBaseModel.h""");
                foreach (var item in info.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO || d.FieldType == EmFieldType.DTOArray))
                {
                    if (item.FieldType == EmFieldType.DTO)
                    {
                        writer.WriteLine(@"#import ""{0}.h""", item.FieldTypeString);
                    }
                    else
                    {
                        writer.WriteLine(@"#import ""{0}.h""", item.FieldTypeString.Replace("[]", String.Empty));
                    }
                }
                writer.WriteLine(@"");
                writer.WriteLine(@"@interface {0} : ServiceBaseModel", fileName);
                writer.WriteLine(@"");
                foreach (var field in info.FieldArray)
                {
                    writer.WriteLine(@"@property(nonatomic,strong){0} *{1}; /**< {2}*/", getFieldTypeOutString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition(), field.caption);
                }
                writer.WriteLine(@"");
                writer.WriteLine(@"");
                writer.WriteLine(@"@end");
            }

            file = new FileStream(string.Format(@"{0}\{1}.m", path, fileName), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  {0}.m", fileName);
                writer.WriteLine(@"//  {0}", Constants.ProjectName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"//  Created by xiao huama on {0}.", DateTime.Now.ToString("yy/MM/dd"));
                writer.WriteLine(@"//  {0}", Constants.CompanyName);
                writer.WriteLine(@"//");
                writer.WriteLine(@"");
                writer.WriteLine(@"#import ""{0}.h""", fileName);
                writer.WriteLine(@"");
                writer.WriteLine(@"@implementation {0}", fileName);
                writer.WriteLine(@"");
                writer.WriteLine(@"- (void)loadDataWithJsonData:(id)jsonData");
                writer.WriteLine(@"{");
                writer.WriteLine(@"    if (![jsonData isKindOfClass:[NSDictionary class]]) {");
                writer.WriteLine(@"        return;");
                writer.WriteLine(@"    }");
                writer.WriteLine(@"    if (!jsonData) {");
                writer.WriteLine(@"        return;");
                writer.WriteLine(@"    }");
                foreach (var field in info.FieldArray)
                {
                    if (isNumberType(field.FieldType))
                    {
                        writer.WriteLine(@"    if (![jsonData[@""{0}""] isKindOfClass:[NSNull class]]) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        self.{0} = jsonData[@""{0}""];", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"    }");
                    }
                    else if (field.FieldType == EmFieldType.DTO)
                    {
                        writer.WriteLine(@"    if ([jsonData[@""{0}""] isKindOfClass:[NSDictionary class]]) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        self.{0} = [[{1} alloc] init];", field.name.ToPrivateDefinition(), field.FieldTypeString);
                        writer.WriteLine(@"        [self.{0} loadDataWithJsonData:jsonData[@""{0}""]];", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"    }");
                    }
                    else
                    {
                        writer.WriteLine(@"    if ([jsonData[@""{0}""] isKindOfClass:[NSArray class]]) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        if ([jsonData[@""{0}""] count]>0) {{", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"            NSMutableArray *tempArray = [NSMutableArray array];");
                        writer.WriteLine(@"            for (id data in jsonData[@""{0}""]) {{", field.name.ToPrivateDefinition());
                        if (isStringArray(field.FieldTypeString.Replace("[]", "")))
                        {
                            writer.WriteLine(@"                [tempArray addObject:data];");
                        }
                        else
                        {
                            writer.WriteLine(@"                [tempArray addObject:[[{0} alloc]initWithJsonData:data]];", field.FieldTypeString.Replace("[]", ""));
                        }
                        writer.WriteLine(@"            }");
                        writer.WriteLine(@"            self.{0} = tempArray;", field.name.ToPrivateDefinition());
                        writer.WriteLine(@"        }");
                        writer.WriteLine(@"    }");
                    }
                }
                writer.WriteLine(@"}");
                writer.WriteLine(@"");
                writer.WriteLine(@"-(NSMutableDictionary *)buildRequestData");
                writer.WriteLine(@"{");
                var lstField = info.FieldArray.FindAll(d => isBaseType(d.FieldType));
                writer.WriteLine(@"    NSMutableDictionary *dic = [super buildVerificationData];");
                if (lstField != null)
                {
                    foreach (var item in lstField)
                    {
                        writer.WriteLine(@"    if (self.{0}) {{", item.name.ToPrivateDefinition());
                        writer.WriteLine(@"        dic[@""{0}""] = self.{0};", item.name.ToPrivateDefinition());
                        writer.WriteLine(@"    }");
                    }
                }

                lstField = info.FieldArray.FindAll(d => isArrayType(d.FieldType));
                if (lstField != null)
                {
                    if (lstField.Count > 0)
                    {
                        foreach (var item in lstField)
                        {
                            if (item.FieldType == EmFieldType.DTOArray)
                            {
                                writer.WriteLine(@"    NSMutableArray *array{0} =[NSMutableArray array];", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    for (BaseModel *mode in self.{0}) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"        [array{0} addObject:[mode buildRequestData]];", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    }");
                                writer.WriteLine(@"    if (array{0}.count>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"        dic[@""{0}""] = array{0};", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    }");
                            }
                            else
                            {
                                writer.WriteLine(@"    if (self.{0}.count>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"        dic[@""{0}""] = self.{0};", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"    }");
                            }
                        }
                    }
                }

                lstField = info.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO);
                if (lstField != null)
                {
                    if (lstField.Count > 0)
                    {
                        foreach (var item in lstField)
                        {
                            writer.WriteLine(@"    NSDictionary *{0}Dic = [self.{0} buildRequestData];", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"    if ({0}Dic) {{", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"        dic[@""{0}""] = {0}Dic;", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"    }");
                        }
                    }
                }

                writer.WriteLine(@"    return dic;");
                writer.WriteLine(@"}");
                writer.WriteLine(@"");
                writer.WriteLine(@"@end");
            }
        }

        private bool isStringArray(string input)
        {
            if (input.Equals("文字"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void getMustInput(StreamWriter writer, HcFieldInfo field)
        {
            switch (field.FieldType)
            {
                case EmFieldType.Boolean:
                case EmFieldType.Byte:
                case EmFieldType.Double:
                case EmFieldType.Integer:
                case EmFieldType.Long:
                case EmFieldType.Timestamp:
                    writer.WriteLine(@"    if (!self.{0}) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"        [muArray addObject:@""{0}不能为空!""];", field.caption);
                    writer.WriteLine(@"        rest = NO;");
                    writer.WriteLine(@"    }");
                    break;
                case EmFieldType.BooleanArray:
                case EmFieldType.DateArray:
                case EmFieldType.DoubleArray:
                case EmFieldType.IntegerArray:
                case EmFieldType.LongArray:
                case EmFieldType.StringArray:
                case EmFieldType.TimeArray:
                case EmFieldType.TimestampArray:
                    writer.WriteLine(@"    if (self.{0}.count<1) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"        [muArray addObject:@""{0}不能为空!""];", field.caption);
                    writer.WriteLine(@"        rest = NO;");
                    writer.WriteLine(@"    }");
                    break;
                case EmFieldType.DTOArray:
                    writer.WriteLine(@"    if (self.{0}.count<1) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"        [muArray addObject:@""{0}不能为空!""];", field.caption);
                    writer.WriteLine(@"        rest = NO;");
                    writer.WriteLine(@"    }");
                    writer.WriteLine(@"    for (SsBaseDataObject *ss in self.{0}) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"        if (![ss checkRequired]) {");
                    writer.WriteLine(@"            tttt;");
                    writer.WriteLine(@"            rest = NO;");
                    writer.WriteLine(@"        }");
                    writer.WriteLine(@"    }");
                    break;
                case EmFieldType.DTO:
                    writer.WriteLine(@"    if (![self.{0} checkRequired]) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"        [muArray addObject:@""{0}不能为空!""];", field.caption);
                    writer.WriteLine(@"        rest = NO;");
                    writer.WriteLine(@"    }");
                    break;
                case EmFieldType.Date:
                case EmFieldType.Time:
                case EmFieldType.String:
                    writer.WriteLine(@"    if (self.{0}.length<1) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"        [muArray addObject:@""{0}不能为空!""];", field.caption);
                    writer.WriteLine(@"        rest = NO;");
                    writer.WriteLine(@"    }");
                    break;
            }
        }

        private bool isArrayType(EmFieldType emFieldType)
        {
            if (emFieldType == EmFieldType.BooleanArray ||
                emFieldType == EmFieldType.DateArray ||
                emFieldType == EmFieldType.DoubleArray ||
                emFieldType == EmFieldType.DTOArray ||
                emFieldType == EmFieldType.IntegerArray ||
                emFieldType == EmFieldType.LongArray ||
                emFieldType == EmFieldType.StringArray ||
                emFieldType == EmFieldType.TimeArray ||
                emFieldType == EmFieldType.TimestampArray)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isBaseType(EmFieldType emFieldType)
        {
            if (emFieldType == EmFieldType.Boolean ||
                emFieldType == EmFieldType.Date ||
                emFieldType == EmFieldType.Double ||
                emFieldType == EmFieldType.Integer ||
                emFieldType == EmFieldType.Long ||
                emFieldType == EmFieldType.String ||
                emFieldType == EmFieldType.Time ||
                emFieldType == EmFieldType.Timestamp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private String getFieldTypeInString(EmFieldType fieldType, String typeString)
        {
            switch (fieldType)
            {
                case EmFieldType.Boolean:
                case EmFieldType.Byte:
                case EmFieldType.Double:
                case EmFieldType.Integer:
                case EmFieldType.Long:
                case EmFieldType.Timestamp:
                    return "NSNumber";
                case EmFieldType.BooleanArray:
                case EmFieldType.DateArray:
                case EmFieldType.DoubleArray:
                case EmFieldType.IntegerArray:
                case EmFieldType.LongArray:
                case EmFieldType.StringArray:
                case EmFieldType.TimeArray:
                case EmFieldType.TimestampArray:
                case EmFieldType.DTOArray:
                    return "NSArray";
                case EmFieldType.DTO:
                    return typeString;
                case EmFieldType.Date:
                case EmFieldType.Time:
                case EmFieldType.String:
                    return "NSString";
                default:
                    return string.Empty;
            }
        }

        private String getFieldTypeOutString(EmFieldType fieldType, String typeString)
        {
            switch (fieldType)
            {
                case EmFieldType.Boolean:
                case EmFieldType.Byte:
                case EmFieldType.Double:
                case EmFieldType.Integer:
                case EmFieldType.Long:
                case EmFieldType.Timestamp:
                    return "NSNumber";
                case EmFieldType.BooleanArray:
                case EmFieldType.DateArray:
                case EmFieldType.DoubleArray:
                case EmFieldType.IntegerArray:
                case EmFieldType.LongArray:
                case EmFieldType.StringArray:
                case EmFieldType.TimeArray:
                case EmFieldType.TimestampArray:
                case EmFieldType.DTOArray:
                    return "NSMutableArray";
                case EmFieldType.DTO:
                    return typeString;
                case EmFieldType.Date:
                case EmFieldType.Time:
                case EmFieldType.String:
                    return "NSMutableString";
                default:
                    return string.Empty;
            }
        }

        private bool isNumberType(EmFieldType fieldType)
        {
            switch (fieldType)
            {
                case EmFieldType.Boolean:
                case EmFieldType.Byte:
                case EmFieldType.Date:
                case EmFieldType.Double:
                case EmFieldType.Integer:
                case EmFieldType.Long:
                case EmFieldType.Time:
                case EmFieldType.Timestamp:
                case EmFieldType.String:
                    return true;
                default:
                    return false;
            }
        }
    }
}
