using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AuthorizeView.xaml
    /// </summary>
    public partial class AuthorizeView : UserControl
    {
        public AuthorizeView()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            var user = DB.Instance.Authorize(login.Text, password.Text);
            if (user != null)
            {
                Guest_Click(sender, e);
            }
            else
            {
                "Пользователь не найден".Msg();
            }
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            this.Locate(new MenuView());
        }
    }
}
