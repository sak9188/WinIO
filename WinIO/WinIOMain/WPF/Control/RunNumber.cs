using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace WinIO.WPF.Control
{
    public class RunNumber : Run
    {
        public int number;
        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                Text = number.ToString() + "次>> ";
            }
        }

        public RunNumber(int num)
        {
            this.Foreground = new SolidColorBrush(Colors.YellowGreen);
            Number = num;
        }

    }
}
