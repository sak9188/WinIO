using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WinIO2
{
    public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate NewButtonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tab_item = item as string;
            if (tab_item != null &&　tab_item == "+")
            {
                return NewButtonTemplate;
            }
            else
            {
                return ItemTemplate;
            }
        }
    }
}
