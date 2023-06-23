using DemoExAgain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    /// Логика взаимодействия для MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DB.Instance.Exit();
            this.Locate(new AuthorizeView());
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            var p = sender.As<MenuItem>().DataContext.As<Product>();
            DB.Instance.AddToOrder(p);
        }

        private void OnViewingOrder(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Просмотр заказа",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            var view = new OrderView(window);
            window.Content = view;
            window.ShowDialog();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
