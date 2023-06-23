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
using System.Windows.Input;

namespace DemoExAgain
{
    static class Helper
    {
        public static void Locate(this DependencyObject obj, UserControl uc)
        {
            Window.GetWindow(obj).Content = uc;
        }

        public static void Msg(this object o, string? title = null, MessageBoxButton button = default, MessageBoxImage image = MessageBoxImage.Error)
        {
            MessageBox.Show(o?.ToString() ?? "null", title, button, image);
        }

        public static T As<T>(this object o) => (T)o;

        public static KeyEventHandler EnterDigits { get; } = (s, e) =>
        {
            if (e.Key is not (>= Key.D0 and <= Key.D9 or
            >= Key.NumPad0 and <= Key.NumPad9 or Key.Back or Key.Left or Key.Right))
            {
                e.Handled = true;
            }
        };
    }
}
