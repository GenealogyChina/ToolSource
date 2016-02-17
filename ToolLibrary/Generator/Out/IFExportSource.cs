using System;
using ToolLibrary.Model.Core;

namespace ToolLibrary.Generator.Out
{
    public interface IFExportSource
    {
        void WriteOutPut(String path, HcSubInfo info);
    }
}
