using SalesToys.Pages;
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

namespace SalesToys
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigated += MainFrame_Navigated;
            MainFrame.Navigate(new LoginPage());
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (User.users == null)
            {
                txtFIO.Text = string.Empty;
                PanelButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtFIO.Text = User.users.FIO;
                PanelButton.Visibility = Visibility.Visible;
            }
        }
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Заглушка");
        }

        private void BtnBackLogin_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                User.users = null;
                MainFrame.Navigate(new LoginPage());
            }
        }
    }
}
