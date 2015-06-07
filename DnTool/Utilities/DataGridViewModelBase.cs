using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Utilities
{
    public class DataGridViewModelBase
    {
        public DataGridViewModelBase()
        {
           
        }

        /// <summary>
        /// 选中改变命令
        /// </summary>
        public RelayCommand<IList> SelectionChangedCommand {  get; set;   }

        /// <summary>
        /// 选中的行数
        /// </summary>
        public int NumberOfItemsSelected { get; set; }
        /// <summary>
        /// 该行是否被选中
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
