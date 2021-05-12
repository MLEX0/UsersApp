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
    /// Логика взаимодействия для AddEditWin.xaml
    /// </summary>
    public partial class AddEditWin : Window
    {
        private Users AddUser { get; set; }
        private Users EditUser { get; set; }

        private bool isEdit;

        public AddEditWin()// Сценарий добавления
        {
            InitializeComponent();

            cmbUserRole.ItemsSource = EF.AppData.context.Role.ToList(); // Заполнение cmbUserRole по базе данных
            cmbUserRole.DisplayMemberPath = "RoleName";
            cmbUserRole.SelectedIndex = 1;

            cmbUserGender.ItemsSource = EF.AppData.context.Gender.ToList(); // Заполнение cmbUserGender по базе данных
            cmbUserGender.DisplayMemberPath = "GenderName";
            cmbUserGender.SelectedIndex = 0;

            txtHeader.Text = "Добавление пользователя";
            isEdit = false;
            AddUser = new Users();
        }

        public AddEditWin(Users user)// Сценарий изменения
        {
            InitializeComponent();

            cmbUserRole.ItemsSource = EF.AppData.context.Role.ToList(); // Заполнение cmbUserRole по базе данных
            cmbUserRole.DisplayMemberPath = "RoleName";
            cmbUserRole.SelectedIndex = 1;

            cmbUserGender.ItemsSource = EF.AppData.context.Gender.ToList(); // Заполнение cmbUserGender по базе данных
            cmbUserGender.DisplayMemberPath = "GenderName";
            cmbUserGender.SelectedIndex = 0;

            txtHeader.Text = "Изменение пользователя";
            isEdit = true;
            AddUser = new Users();
            EditUser = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEdit == true)
            {
                txtFirstName.Text = EditUser.FName;
                txtLastName.Text = EditUser.LName;
                txtMiddleName.Text = EditUser.MName;
                txtLogin.Text = EditUser.Login;
                txtPassword.Text = EditUser.Password;
                cmbUserGender.SelectedIndex = Convert.ToInt32(EditUser.IdGender) - 1;
                cmbUserRole.SelectedIndex = Convert.ToInt32(EditUser.IdRole) - 1;
                dpcUserBirthday.SelectedDate = EditUser.Birthday;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    MessageBox.Show("Поле Фамилия не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("Поле Имя не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLogin.Text))
                {
                    MessageBox.Show("Поле Логин не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Поле Логин не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (dpcUserBirthday.SelectedDate == null)
                {
                    MessageBox.Show("Поле Дата рождения не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var uniqueUser = EF.AppData.context.Users.ToList().Where(i => i.Login == txtLogin.Text).FirstOrDefault();

                if (isEdit == false)
                {
                    if (uniqueUser == null)
                    {
                        var result = MessageBox.Show("Вы уверены?", "Добавление нового сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            AddUser.LName = txtLastName.Text;
                            AddUser.FName = txtFirstName.Text;
                            if (!string.IsNullOrWhiteSpace(txtMiddleName.Text))
                            {
                                AddUser.MName = txtMiddleName.Text;
                            }
                            AddUser.Login = txtLogin.Text;
                            AddUser.Password = txtPassword.Text;
                            AddUser.IdRole = cmbUserRole.SelectedIndex + 1;
                            AddUser.IdGender = cmbUserGender.SelectedIndex + 1;
                            AddUser.Birthday = dpcUserBirthday.SelectedDate.Value;

                            EF.AppData.context.Users.Add(AddUser);
                            EF.AppData.context.SaveChanges();
                            MessageBox.Show("Сотрудник добавлен");
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Логин занят", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                else if (isEdit == true)
                {
                    if (uniqueUser == null || uniqueUser.Login == EditUser.Login)
                    {
                        var result = MessageBox.Show("Вы уверены?", "Изменение данных сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            EditUser.FName = txtFirstName.Text;
                            EditUser.LName = txtLastName.Text;
                            if (!string.IsNullOrWhiteSpace(txtMiddleName.Text))
                            {
                                EditUser.MName = txtMiddleName.Text;
                            }
                            EditUser.Login = txtLogin.Text;
                            EditUser.Password = txtPassword.Text;
                            EditUser.IdRole = cmbUserRole.SelectedIndex + 1;
                            EditUser.IdGender = cmbUserGender.SelectedIndex + 1;
                            EditUser.Birthday = dpcUserBirthday.SelectedDate.Value;

                            EF.AppData.context.SaveChanges();
                            MessageBox.Show("Изменения сохранены");
                            this.Close();
                        }
                    }

                    else
                    {
                        MessageBox.Show("Логин занят", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
