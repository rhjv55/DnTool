using DnTool.Models;
using DnTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.ViewModels
{
    public class InfoViewModel:ViewModelBase
    {
        public Point CurrentPoint { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        private bool _isSelected;

	public bool IsSelected
	{
		get { return _isSelected;}
		set 
                { 
 		  _isSelected = value;
                  this.OnPropertyChanged("IsSelected");
                }
	}
	
 
    }
}
