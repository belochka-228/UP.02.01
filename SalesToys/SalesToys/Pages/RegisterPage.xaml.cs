using SalesToys.Error;
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

namespace SalesToys.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPag.xaml
    /// </summary>
    public partial class RegisterPag : Page
    {
        public RegisterPag()
        {
            InitializeComponent();
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            string fio =txtFIO.Text.Trim();
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;
            string password2 = txtPassword2.Password;

            if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(password2))
            {
                Message.FillingInFields();
                return;
            }
            var human = Core.Context.Users.Any(u => u.Login == login);
            if (human)
            {
                MessageBox.Show("Пользователь с данным логином существует!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(password != password2)
            {
                MessageBox.Show("Пароли не совпадают!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            var newUser = new Users
            {
                IDRoles = 3,
                FIO = fio,
                Login = login,
                Password = password
            };
            Core.Context.Users.Add(newUser);
            try
            {
                Core.Context.SaveChanges();
                MessageBox.Show("Успешная регистрация, теперь войдите!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            NavigationService.Navigate(new LoginPage());
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
