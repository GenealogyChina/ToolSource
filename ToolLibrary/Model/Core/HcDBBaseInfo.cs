using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolLibrary.Model.Core
{
    public abstract class HcDBBaseInfo
    {
        public string Name { get; protected set; }

        public List<HcColumnInfo> Columns { get; protected set; }
    }
}
