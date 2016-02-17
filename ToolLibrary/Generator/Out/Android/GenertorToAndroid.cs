using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolLibrary.Model.Core;
using ToolLibrary.Type;
using Utility.Extension;

namespace ToolLibrary.Generator.Out.Android
{
    public class GenertorToAndroid : IFExportSource
    {
        private Dictionary<string, string> dicMessage = new Dictionary<string, string>();

        public void WriteOutPut(String path, HcSubInfo info)
        {
            var requestPath = path + @"\Android\src\com\xl\frame\requester\" + info.Name;
            DirectoryInfo newFolder = null;
            if (!Directory.Exists(requestPath))
            {
                Directory.CreateDirectory(requestPath);
            }
            newFolder = new DirectoryInfo(requestPath);

            foreach (var service in info.ServiceArray)
            {
                createRequester(newFolder.FullName, service, info.Name);
            }

            var outEntityPath = path + @"\Android\src\com\xl\frame\entity";
            if (!Directory.Exists(outEntityPath))
            {
                Directory.CreateDirectory(outEntityPath);
            }
            newFolder = new DirectoryInfo(outEntityPath);

            foreach (var service in info.ServiceArray)
            {
                createOutEntity(newFolder.FullName, service);
            }

            if (info.DTOArray != null)
            {
                foreach (var dto in info.DTOArray)
                {
                    writeDTO(newFolder, dto);
                }
            }
        }

        private void writeDTO(DirectoryInfo newFolder, HcDTOInfo dto)
        {
            var file = new FileStream(string.Format(@"{0}\{1}.java", newFolder.FullName, dto.Name), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * ");
                writer.WriteLine(@" */");
                writer.WriteLine(@"package com.xl.frame.entity;");
                writer.WriteLine(@"");
                if (dto.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTOArray).Count > 0)
                {
                    writer.WriteLine(@"import java.util.ArrayList;");
                }
                writer.WriteLine(@"import java.util.HashMap;");
                if (dto.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTOArray).Count > 0)
                {
                    writer.WriteLine(@"import java.util.List;");
                }
                writer.WriteLine(@"");
                dto.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO).ForEach(item =>
                {
                    writer.WriteLine(@"import com.xl.frame.entity.{0};", item.FieldTypeString);
                });

                writer.WriteLine(@"");
                writer.WriteLine(@"public class {0} extends BaseEntity implements IBaseEntity {{", dto.Name);
                writer.WriteLine(@"");
                writer.WriteLine(@"	/**");
                writer.WriteLine(@"	 * ");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	private static final long serialVersionUID = 1L;");
                writer.WriteLine(@"");
                foreach (var field in dto.FieldArray)
                {
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public {0} {1}; // {2}", getFieldTypeInString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition(), field.caption);
                }
                writer.WriteLine(@"");
                writer.WriteLine(@"	@Override");
                writer.WriteLine(@"	public HashMap<String, Object> buildRequestData() {");
                writer.WriteLine(@"		HashMap<String, Object> params = new HashMap<String, Object>({0});", dto.FieldArray.Count);

                var lstField = dto.FieldArray.FindAll(d => isBaseType(d.FieldType));
                if (lstField != null)
                {
                    foreach (var item in lstField)
                    {
                        writer.WriteLine(@"		params.put(""{0}"", this.{1});", item.name, item.name.ToPrivateDefinition());
                    }
                }

                lstField = dto.FieldArray.FindAll(d => isArrayType(d.FieldType));
                if (lstField != null)
                {
                    if (lstField.Count > 0)
                    {
                        foreach (var item in lstField)
                        {
                            if (item.FieldType == EmFieldType.DTOArray)
                            {
                                writer.WriteLine(@"     List<HashMap<String, Object>> array{0} = new ArrayList<HashMap<String, Object>>();", item.name);
                                writer.WriteLine(@"     for (IBaseEntity entity : this.{0}) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"         array{0}.add(entity.buildRequestData());", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"		}");
                                writer.WriteLine(@"     if (array{0}.size()>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"         params.put(""{0}"",array{1});", item.name, item.name.ToPrivateDefinition());
                                writer.WriteLine(@"     }");
                            }
                            else
                            {
                                writer.WriteLine(@"		if (this.{0}.length>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"         params.put(""{0}"", this.{1});", item.name, item.name.ToPrivateDefinition());
                                writer.WriteLine(@"		}");
                            }
                        }
                    }
                }

                lstField = dto.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO);
                if (lstField != null)
                {
                    if (lstField.Count > 0)
                    {
                        foreach (var item in lstField)
                        {
                            writer.WriteLine(@"		HashMap<String, Object> map{0} = this.{1}.buildRequestData();", item.name, item.name.ToPrivateDefinition());
                            writer.WriteLine(@"		if (map{0} != null) {{", item.name);
                            writer.WriteLine(@"		    params.put(""{0}"", map{0});", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"		}");
                        }
                    }
                }
                writer.WriteLine(@"		return params;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"");
                writer.WriteLine(@"	@Override");
                writer.WriteLine(@"	public Boolean checkInputData() {");
                foreach (var item in dto.FieldArray)
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

                writer.WriteLine(@"		return true;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"}");
            }
        }

        private void createOutEntity(string path, HcServiceInfo service)
        {
            var file = new FileStream(string.Format(@"{0}\{1}Entity.java", path, service.Name), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(@"/**");
                writer.WriteLine(@" * ");
                writer.WriteLine(@" */");
                writer.WriteLine(@"package com.xl.frame.entity;");
                writer.WriteLine(@"");
                service.OutDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO).ForEach(item =>
                {
                    writer.WriteLine(@"import com.xl.frame.entity.{0};", item.FieldTypeString);
                });
                writer.WriteLine(@"");
                writer.WriteLine(@"public class {0}Entity extends BaseEntity {{", service.Name);
                writer.WriteLine(@"");
                writer.WriteLine(@"	/**");
                writer.WriteLine(@"	 * ");
                writer.WriteLine(@"	 */");
                writer.WriteLine(@"	private static final long serialVersionUID = 1L;");
                writer.WriteLine(@"");
                foreach (var field in service.OutDTO.FieldArray)
                {
                    writer.WriteLine(@"");
                    writer.WriteLine(@"	public {0} {1}; // {2}", getFieldTypeInString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition(), field.caption);
                }

                writer.WriteLine(@"}");
            }
        }

        private void createRequester(string path, HcServiceInfo service, string info)
        {
            var file = new FileStream(string.Format(@"{0}\{1}Requester.java", path, service.Name), FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
            {

                writer.WriteLine(@"package com.xl.frame.requester.{0};", info);
                writer.WriteLine(@"");
                if (service.InDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTOArray).Count > 0)
                {
                    writer.WriteLine(@"import java.util.ArrayList;");
                }
                writer.WriteLine(@"import java.util.HashMap;");
                if (service.InDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTOArray).Count > 0)
                {
                    writer.WriteLine(@"import java.util.List;");
                }
                writer.WriteLine(@"import java.util.Map;");
                writer.WriteLine(@"");
                writer.WriteLine(@"import com.xl.frame.core.lib.gson.reflect.TypeToken;");
                writer.WriteLine(@"import com.xl.frame.core.lib.volley.AuthFailureError;");
                writer.WriteLine(@"import com.xl.frame.core.lib.volley.Response;");
                writer.WriteLine(@"import com.xl.frame.core.lib.volley.VolleyError;");
                writer.WriteLine(@"import com.xl.frame.core.net.GsonRequest;");
                writer.WriteLine(@"import com.xl.frame.entity.RetEntity;");
                writer.WriteLine(@"import com.xl.frame.entity.{0}Entity;", service.Name);
                service.InDTO.FieldArray.FindAll(d => d.FieldType == EmFieldType.DTO).ForEach(item => {
                    writer.WriteLine(@"import com.xl.frame.entity.{0};", item.FieldTypeString);
                });
                writer.WriteLine(@"import com.xl.frame.requester.BaseRequester;");

                writer.WriteLine(@"");
                writer.WriteLine(@"public class {0}Requester extends BaseRequester<{0}Entity> {{", service.Name);
                writer.WriteLine(@"");
                foreach (var field in service.InDTO.FieldArray)
                {
                    writer.WriteLine(@"	private {0} {1} = null; //{2}", getFieldTypeInString(field.FieldType, field.FieldTypeString), field.name.ToPrivateDefinition(), field.caption);
                }
                writer.WriteLine(@"");
                writer.WriteLine(@"	// {0}", service.Caption);

                var inputParam = new StringBuilder();
                service.InDTO.FieldArray.ForEach(item =>
                {
                    if (inputParam.Length > 0)
                        inputParam.Append(", ");
                    inputParam.Append(string.Format("final {0} {1}", getFieldTypeInString(item.FieldType, item.FieldTypeString), item.name.ToPrivateDefinition()));
                });


                writer.WriteLine(@"	public void postData({0}) {{", inputParam.ToString());
                service.InDTO.FieldArray.ForEach(item =>
                {
                    writer.WriteLine(@"		this.{0} = {0};", item.name.ToPrivateDefinition());
                });
                writer.WriteLine(@"");
                writer.WriteLine(@"		if (this.checkInputData()) {");
                writer.WriteLine(@"			final Map<String, Object> params = buildVerificationData();");
                writer.WriteLine(@"			GsonRequest<RetEntity<{0}Entity>> req = new GsonRequest<RetEntity<{0}Entity>>(", service.Name);
                writer.WriteLine(@"					getUrl(""{0}""),", service.Name);
                writer.WriteLine(@"					new Response.Listener<RetEntity<{0}Entity>>() {{", service.Name);
                writer.WriteLine(@"						");
                writer.WriteLine(@"						@Override");
                writer.WriteLine(@"						public void onResponse(RetEntity<{0}Entity> response) {{", service.Name);
                writer.WriteLine(@"							if (response.success) {");
                writer.WriteLine(@"								if (callback != null) {");
                writer.WriteLine(@"									callback.onCallback(response.result);");
                writer.WriteLine(@"								}");
                writer.WriteLine(@"							} else {");
                writer.WriteLine(@"								if (callback != null) {");
                writer.WriteLine(@"									callback.onErrorCallback(response.getErrorMsg());");
                writer.WriteLine(@"								}");
                writer.WriteLine(@"							}");
                writer.WriteLine(@"						}");
                writer.WriteLine(@"");
                writer.WriteLine(@"					}, new Response.ErrorListener() {");
                writer.WriteLine(@"						@Override");
                writer.WriteLine(@"						public void onErrorResponse(VolleyError error) {");
                writer.WriteLine(@"							if (callback != null) {");
                writer.WriteLine(@"								callback.onErrorCallback(error.getMessage());");
                writer.WriteLine(@"							}");
                writer.WriteLine(@"						}");
                writer.WriteLine(@"					}) {");
                writer.WriteLine(@"");
                writer.WriteLine(@"				@Override");
                writer.WriteLine(@"				public Map<String, Object> getParams() throws AuthFailureError {");
                writer.WriteLine(@"					return params;");
                writer.WriteLine(@"				}");
                writer.WriteLine(@"			};");
                writer.WriteLine(@"			");
                writer.WriteLine(@"			req.setTypeOfT(new TypeToken<RetEntity<{0}Entity>>() {{}}.getType());", service.Name);
                writer.WriteLine(@"			mNetManager.addToRequestQueue(req, ""{0}Requester"");", service.Name);
                writer.WriteLine(@"		} else {");
                writer.WriteLine(@"			if (callback != null) {");
                writer.WriteLine(@"				callback.onErrorCallback(this.errorMsg);");
                writer.WriteLine(@"			}");
                writer.WriteLine(@"		}");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"");
                writer.WriteLine(@"	@Override");
                writer.WriteLine(@"	public HashMap<String, Object> buildRequestData() {");
                writer.WriteLine(@"		HashMap<String, Object> params = new HashMap<String, Object>({0});", service.InDTO.FieldArray.Count);

                var lstField = service.InDTO.FieldArray.FindAll(d => isBaseType(d.FieldType));
                if (lstField != null)
                {
                    foreach (var item in lstField)
                    {
                        writer.WriteLine(@"		params.put(""{0}"", this.{1});", item.name, item.name.ToPrivateDefinition());
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
                                writer.WriteLine(@"     List<HashMap<String, Object>> array{0} = new ArrayList<HashMap<String, Object>>();", item.name);
                                writer.WriteLine(@"     for (IBaseEntity entity : this.{0}) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"         array{0}.add(entity.buildRequestData());", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"		}");
                                writer.WriteLine(@"     if (array{0}.size()>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"         params.put(""{0}"",array{1});", item.name, item.name.ToPrivateDefinition());
                                writer.WriteLine(@"     }");
                            }
                            else
                            {
                                writer.WriteLine(@"		if (this.{0}.length>0) {{", item.name.ToPrivateDefinition());
                                writer.WriteLine(@"         params.put(""{0}"", this.{1});", item.name, item.name.ToPrivateDefinition());
                                writer.WriteLine(@"		}");
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
                            writer.WriteLine(@"		HashMap<String, Object> map{0} = this.{1}.buildRequestData();", item.name, item.name.ToPrivateDefinition());
                            writer.WriteLine(@"		if (map{0} != null) {{", item.name);
                            writer.WriteLine(@"		    params.put(""{0}"", map{0});", item.name.ToPrivateDefinition());
                            writer.WriteLine(@"		}");
                        }
                    }
                }
                writer.WriteLine(@"		return params;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"");
                writer.WriteLine(@"	@Override");
                writer.WriteLine(@"	public Boolean checkInputData() {");
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

                writer.WriteLine(@"		return true;");
                writer.WriteLine(@"	}");
                writer.WriteLine(@"}");
            }
        }

        private String getFieldTypeInString(EmFieldType fieldType, String typeString)
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
                    writer.WriteLine(@"		if (this.{0} == null) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"			super.errorMsg = ""{0}不能为空!"";", field.caption);
                    writer.WriteLine(@"			return false;");
                    writer.WriteLine(@"		}");
                    break;
                case EmFieldType.BooleanArray:
                case EmFieldType.DateArray:
                case EmFieldType.DoubleArray:
                case EmFieldType.IntegerArray:
                case EmFieldType.LongArray:
                case EmFieldType.StringArray:
                case EmFieldType.TimeArray:
                case EmFieldType.TimestampArray:
                    writer.WriteLine(@"		if (this.{0}.length<1) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"			super.errorMsg = ""{0}不能为空!"";", field.caption);
                    writer.WriteLine(@"			return false;");
                    writer.WriteLine(@"		}");
                    break;
                case EmFieldType.DTOArray:
                    writer.WriteLine(@"		if (this.{0}.length<1) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"			super.errorMsg = ""{0}不能为空!"";", field.caption);
                    writer.WriteLine(@"			return false;");
                    writer.WriteLine(@"		}");
                    writer.WriteLine(@"		for (BaseEntity entity ： self.{0}) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"			if (!entity.checkInputData()) {");
                    writer.WriteLine(@"             super.errorMsg = entity.errorMsg;");
                    writer.WriteLine(@"             return false;");
                    writer.WriteLine(@"			}");
                    writer.WriteLine(@"		}");
                    break;
                case EmFieldType.DTO:
                    writer.WriteLine(@"		if (!this.{0}.checkInputData()) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"			super.errorMsg = ""{0}不能为空!"";", field.caption);
                    writer.WriteLine(@"			return false;");
                    writer.WriteLine(@"		}");
                    break;
                case EmFieldType.Date:
                case EmFieldType.Time:
                case EmFieldType.String:
                    writer.WriteLine(@"		if (this.{0} == null) {{", field.name.ToPrivateDefinition());
                    writer.WriteLine(@"			super.errorMsg = ""{0}不能为空!"";", field.caption);
                    writer.WriteLine(@"			return false;");
                    writer.WriteLine(@"		}");
                    break;
            }
        }


    }
}
