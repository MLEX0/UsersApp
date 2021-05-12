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
using System.Windows.Shapes;
using UsersApp.EF;

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для ManagerWin.xaml
    /// </summary>
    public partial class ManagerWin : Window
    {
        public ManagerWin()
        {
            InitializeComponent();
        }

        public ManagerWin(Users user)
        {
            InitializeComponent();
            lvUsers.ItemsSource = EF.AppData.context.Users.ToList();
        }
    }
}
