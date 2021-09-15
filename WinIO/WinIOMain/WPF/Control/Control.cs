 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WinIO.WPF.Control
{
    public static class Control
    {
        public static ResourceDictionary Resources = GetResourceDictionaryByURI(new Uri(@"pack://application:,,,/wpf/control/control.xaml"));

        public static ResourceDictionary GetResourceDictionaryByURI(Uri uri)
        {
             ResourceDictionary rd = new ResourceDictionary();
            rd.Source = uri;
            return rd;
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        public static T GetElementUnderMouse<T>() where T : UIElement
        {
            return FindVisualParent<T>(Mouse.DirectlyOver as UIElement);
        }
    }
}
