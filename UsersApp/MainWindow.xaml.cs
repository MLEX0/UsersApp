using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using UsersApp.EF;
using static UsersApp.EF.AppData;

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int cpActivate = 0;
        int errorCounter = 0;
        int errorOfRead = 0;

        public MainWindow()
        {
            InitializeComponent();


            if (File.Exists("File.txt") == true)// защита от дауна
            {
                if (FileSaveClass.FileRead("File.txt") != null)
                {
                    try
                    {
                        Convert.ToInt32(FileSaveClass.FileRead("File.txt")); // проверка правильности данных в файле
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка сохранения пользователя, повторите вход!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        errorOfRead = 1;
                        File.Delete("File.txt");
                    }

                    if (errorOfRead == 0)
                    {
                        if (Convert.ToInt32(FileSaveClass.FileRead("File.txt")) > context.Users.Count() || Convert.ToInt32(FileSaveClass.FileRead("File.txt")) < 0)// Проверка id 
                        {
                            MessageBox.Show("Сохранённый пользователь перестал существовать!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            File.Delete("File.txt");
                        }
                        else
                        {
                            cbxRemind.IsChecked = true;
                            int saveId = Convert.ToInt32(FileSaveClass.FileRead("File.txt"));
                            var IdUser = context.Users.ToList().Where(u => u.IdUser == saveId).FirstOrDefault();
                            txtLogin.Text = IdUser.Login;
                            pswPassword.Password = IdUser.Password;
                        }
                    }
                }
            }
            else if (File.Exists("File.txt") == false)// Если файла не существует, создаёт файл
            {
                File.Create("File.txt");
            }
        }

        private void CapchaGet()//Получает новую капчу и присваивает её текстовому полю! 
        {
            string CpString = "";
            CpString = CapchaGenClass.CapchaGenerate();
            txbCapchaEnter.Text = CpString;
        }

        private void CapchaShow()//открывает капчу на окне
        {
            btnClose1.Visibility = Visibility.Hidden;
            btnLogin1.Visibility = Visibility.Hidden;
            btnClose2.Visibility = Visibility.Visible;
            btnLogin2.Visibility = Visibility.Visible;
            txtCapcha.Visibility = Visibility.Visible;
            txbCapcha.Visibility = Visibility.Visible;
            txbCapchaEnter.Visibility = Visibility.Visible;
            btnCapchaReboot.Visibility = Visibility.Visible;
            brdCapcha.Visibility = Visibility.Visible;
        }

        private void Login()// Метод входа в приложение
        {
            var user = context.Users.ToList().Where(u => u.Login == txtLogin.Text && u.Password == pswPassword.Password).FirstOrDefault();// Поиск по логину и паролю

            if (user != null && txbCapchaEnter.Text.ToLower() == txtCapcha.Text.ToLower())// проверка правильности ввода капчи и пароля
            {
                if (File.Exists("File.txt") == true)// Проверка существования файла!
                {
                    if (cbxRemind.IsChecked == true && FileSaveClass.FileRead("File.txt") != null && File.Exists("File.txt") == true)
                    {
                        FileSaveClass.FileWrite(Convert.ToString(user.IdUser), "File.txt");// записывает id пользователя в файл
                    }
                    else if (cbxRemind.IsChecked == false)// удаление файла 
                    {
                        File.Delete("File.txt");
                    }
                }
                else
                {
                    if (cbxRemind.IsChecked == true && File.Exists("File.txt") == false)// Полная шляпа, когда пользователь трогает сраный файл!!!
                    {
                        MessageBox.Show("Внимание! \nИсполняемый файл занят системным процессом! " +
                            "\nПри следующей авторизации вам придётся ещё раз ввести ваши данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                switch (user.IdRole)
                {
                        case 1:
                        AdminWin adminWin = new AdminWin(user);
                        this.Hide();
                        adminWin.ShowDialog();
                        this.Close();
                        break;

                        case 2:
                        WorkWin workwin = new WorkWin(user); // Переход на рабочее окно пользователя
                        this.Hide();
                        workwin.ShowDialog();
                        this.Close();
                        break;

                        case 3:
                        ManagerWin managerwin = new ManagerWin(user); // Переход на рабочее окно пользователя
                        this.Hide();
                        managerwin.ShowDialog();
                        this.Close();
                        break;
                }


            }
            else// при неправильном вводе пароля
            {
                MessageBox.Show("Неправильный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                errorCounter++;// счёт ошибок

                if (txbCapchaEnter.Text.ToLower() != txtCapcha.Text.ToLower() && cpActivate == 1)// неправильно введена капча
                {
                    MessageBox.Show("Неправильно введена капча!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (errorCounter > 2)// Открытие капчи при трёх ошибках
                {
                    CapchaShow();
                    cpActivate = 1;
                }
            }

            if (cpActivate == 1)// Получение новой капчи при первом открытии
            {
                CapchaGet();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();// Вход по кнопке
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрытие по кнопке
        }

        private void btnCapchaReboot_Click(object sender, RoutedEventArgs e)
        {
            CapchaGet(); // Обновление капчи по нажатию кнопки
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)// Выход при нажатии Esc
            {
                this.Close();
            }

            if (e.Key == Key.Enter)// Вход при нажатии Enter
            {
                Login();
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)// При нажатии на текст "Запомнить меня" комбобокс маеняется
            {
                if (cbxRemind.IsChecked == false)
                {
                    cbxRemind.IsChecked = true;
                }
                else
                {
                    cbxRemind.IsChecked = false;
                }
            }
        }
    }
}
