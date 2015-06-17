using DnTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Models
{
    public class MemoryAddress
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Offset { get; set; }

        public MemoryAddress BaseAddress { get; set; }

        public override string ToString()
        {
            string result = Offset;
            if (BaseAddress != null)
            {
                result = BaseAddress.Name == "游戏基址"
                    ? AnyRadixConvert._10To16(AnyRadixConvert._16To10(BaseAddress.Offset) + AnyRadixConvert._16To10(Offset))
                    : string.Format("[{0}]+{1}",BaseAddress.ToString(),Offset);
            }
            return string.IsNullOrEmpty(result) ? "0" : result.StartsWith("[") || result[0] == '0' ? result : '0' + result;
        }
    }
}
