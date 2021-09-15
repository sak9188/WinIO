using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinIO.WPF.Control
{
    public class IOMenuItem : System.Windows.Controls.MenuItem
    {
        private PyObject onClick = null;

        public PyObject PyOnClick
        {
            set
            {
                onClick = value;
            }

            get
            {
                return onClick;
            }
        }

        public IOMenuItem() : base()
        {
        }

        public void AddMenuItem(IOMenuItem subitem)
        {
            this.Items.Add(subitem);
        }

        public void SetForeColor(string color)
        {
            try
            {
                this.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            }
            catch (FormatException)
            {
                Debug.WriteLine("wrong color string");
            }
        }

        //protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);
        //    // Debug.WriteLine("something");
        //}

    }
}
