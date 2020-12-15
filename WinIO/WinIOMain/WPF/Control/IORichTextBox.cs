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

    }
}
