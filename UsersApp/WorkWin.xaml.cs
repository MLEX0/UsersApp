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
using static UsersApp.EF.AppData;

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для WorkWin.xaml
    /// </summary>
    public partial class WorkWin : Window
    {
        public WorkWin(EF.Users user)
        {
            InitializeComponent();

            txtId.Text = Convert.ToString(user.IdUser);
            txtName.Text = $"{user.LName} {user.FName} {user.MName}";
            txtLogin.Text = user.Login;
            txtPassword.Text = user.Password;
            txtGender.Text = user.Gender.GenderName;
            txtBirthDay.Text = $"{user.Birthday}";
            txtRole.Text = $"{user.Role.RoleName}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
