using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void AddMenuItem(IOMenuItem subitem)
        {
            this.Items.Add(subitem);
        }

    }
}
