using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoExAgain.Views
{
    /// <summary>
    /// Логика взаимодействия для TicketView.xaml
    /// </summary>
    public partial class TicketView : UserControl
    {
        private readonly Window _window;
        public TicketView(Window window)
        {
            InitializeComponent();
            _window = window;
        }

        private void OnSaving(object sender, RoutedEventArgs e)
        {
            var pd = new PrintDialog
            {
                PrintQueue = pdfPrintQueue
            };
            pd.PrintVisual(ticket, "Талон");
            _window.Close();
        }

        private void OnClosing(object sender, RoutedEventArgs e)
        {
            _window.Close();
        }

        private static readonly PrintQueue pdfPrintQueue = new(new(), "Microsoft Print to PDF");
    }
}
