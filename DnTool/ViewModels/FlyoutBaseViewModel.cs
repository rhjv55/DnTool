using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnTool.Utilities;
using MahApps.Metro.Controls;
namespace DnTool.ViewModels
{
    public abstract class FlyoutBaseViewModel:NotifyPropertyChanged
    {
        private string header;
        private bool isOpen;
        private Position position;

        public event EventHandler OpendEvent;

        public Position Position
        {
            get { return position; }
            set
            {
                if (value == this.position)
                    return;
                base.SetProperty(ref position, value, () => this.Position);
            }
        }
        



        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                if (value.Equals(this.isOpen))
                {
                    return;
                }
                base.SetProperty(ref isOpen, value, () => this.IsOpen);
                if (this.OpendEvent != null&&this.isOpen)
                    OpendEvent.Invoke(this,new EventArgs()) ;
            }
        }
        




        public string Header
        {
            get { return header; }
            set
            {
                if (value == this.header)
                    return;
                base.SetProperty(ref header, value, () => this.Header);
            }
        }
        
    }
}
