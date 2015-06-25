using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Models
{
    public class MallThing
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Value { get; set; }
        public bool CanUseLB { get; set; }
    }
}
