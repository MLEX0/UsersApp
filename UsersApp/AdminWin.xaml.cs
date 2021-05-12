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
    /// Логика взаимодействия для AdminWin.xaml
    /// </summary>
    public partial class AdminWin : Window
    {
        Users currentUser { get; set; }

        public AdminWin()
        {
            InitializeComponent();
        }

        public AdminWin(Users user)
        {
            InitializeComponent();
            currentUser = user;
            lvUsers.ItemsSource = EF.AppData.context.Users.ToList();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem is Users selectuser)
            {
                var result = MessageBox.Show("Удалить выбранного пользователя?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (currentUser != selectuser)
                    {
                        EF.AppData.context.Users.Remove(selectuser);
                        EF.AppData.context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Вы не можете удалить текущего пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана");
            }


            lvUsers.ItemsSource = EF.AppData.context.Users.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditWin addEditWin = new AddEditWin();
            this.Opacity = 0.5;
            addEditWin.ShowDialog();
            lvUsers.ItemsSource = EF.AppData.context.Users.ToList();
            this.Opacity = 1;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem is Users selectuser)
            {
                AddEditWin addEditWin = new AddEditWin(selectuser);
                this.Opacity = 0.5;
                addEditWin.ShowDialog();
                lvUsers.ItemsSource = EF.AppData.context.Users.ToList();
                this.Opacity = 1;
            }
            else
            {
                MessageBox.Show("Запись не выбрана");
            }
        }
    }
}
