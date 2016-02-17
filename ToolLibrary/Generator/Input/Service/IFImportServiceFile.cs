using System;
using System.Collections.Generic;
using ToolLibrary.Model.Core;

namespace ToolLibrary.Generator.Input.Service
{
    public interface IFImportServiceFile
    {
        void ReadServiceInputFolder(String path, Dictionary<String, HcSubInfo> dic);
        void ReadDTOInputFolder(String path, Dictionary<String, HcSubInfo> dic);
    }
}
