using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinIO.WPF.Control
{
    public class IORichTextBox : System.Windows.Controls.RichTextBox
    {
        private double minPageWidth = 0;

        public double MinPageWidth
        {
            set
            {
                if(value > minPageWidth)
                {
                    minPageWidth = value;

                    this.Document.MinPageWidth = value;
                }
            }

            get
            {
                return minPageWidth;
            }
        }

        private PyObject pyKeyDown;

        public PyObject PyKeyDown
        {
            set
            {
                pyKeyDown = value;
            }
            get
            {
                return pyKeyDown;
            }
        }

        public IORichTextBox()
        {
            this.KeyDown += RichTextBox_KeyDown;
        }

        private void RichTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (pyKeyDown != null)
            {
                using(Py.GIL())
                {
                    pyKeyDown.Invoke(e.Key.ToPython(), e.Key.ToString().ToPython());
                }
            }
        }
    }
}
