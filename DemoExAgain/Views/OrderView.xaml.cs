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
    /// Логика взаимодействия для OrderView.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        private readonly Window _window;
        private readonly DB db;
        public OrderView(Window window)
        {
            InitializeComponent();
            _window = window;
            db = DB.Instance;
            Calculate();
        }

        private void Calculate()
        {
            sum.Text = "Сумма: " + db.ProductsInOrder.Sum(p => p.Cost).ToString("#.## РУБ.");
            discount.Text = "Скидка: " + db.ProductsInOrder.Sum(p => p.Cost / 100 * p.Discount).ToString("#.## РУБ.");
        }

        private void OnPlacingOrder(object sender, RoutedEventArgs e)
        {
            var pp = pickupPoints.SelectedItem.As<PickupPoint>();
            var products = db.Order.ToList();
            var order = db.FormOrder(pp);

            int days = order.DeliveryDate.Subtract(order.OrderDate).Days;
            var dataContext = new
            {
                Number = order.Id,
                Products = products,
                Sum = sum.Text,
                Discount = discount.Text,
                PickupPointName = $"{pp.Address}, {pp.Index}",
                order.CodeToGet,
                DeliveryTime = $"{days} дн{(days is 3 ? 'я' : "ей")}"
            };
            _window.Content = new TicketView(_window) { DataContext = dataContext };
        }

        private void LostFocusChangeCount(object sender, RoutedEventArgs e)
        {
            var tb = sender.As<TextBox>();
            var pwc = tb.DataContext.As<ProductWithCount>();
            db.ChangeProductCount(pwc);
            if (db.Order.Count != 0)
            {
                Calculate();
            }
            else
            {
                _window.Close();
            }
        }

        private void OnRemoving(object sender, RoutedEventArgs e)
        {
            var btn = sender.As<Button>();
            var pwc = btn.DataContext.As<ProductWithCount>();
            pwc.Count = 0;
            db.ChangeProductCount(pwc);
            if (db.Order.Count != 0)
            {
                Calculate();
            }
            else
            {
                _window.Close();
            }
        }
    }
}
