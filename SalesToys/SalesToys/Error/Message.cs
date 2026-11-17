using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SalesToys.Error
{
    public static class Message
    {
        public static void FillingInFields()
        {
            MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void UserDoesNotExist()
        {
            MessageBox.Show("Пользователя с данным логином не существует!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void SuccessfulAuthorization()
        {
            MessageBox.Show("Успешный вход!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }
}
