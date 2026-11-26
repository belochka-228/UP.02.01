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
            if (UserData.users == null)
            {
                txtFIO.Text = string.Empty;
                BtnButton.Visibility = Visibility.Collapsed;
                if(e.Content is LoginPage || e.Content is RegisterPag)
                {
                    btn_Back.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btn_Back.Visibility= Visibility.Visible;
                }
            }
            else
            {
                txtFIO.Text = UserData.users.FIO;
                BtnButton.Visibility = Visibility.Visible;
                btn_Back.Visibility = Visibility.Collapsed;
            }
        }
        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var page = e.Content as Page;
            string pageName = page.GetType().Name;
            if (page != null)
            {
                string title = "";

                switch (pageName)
                {
                    case "LoginPage": title = ""; break;
                    case "RegisterPag": title = ""; break;
                    case "CatalogPage": title = ""; break;
                    case "_3RestoranPage": title = "Выбор ресторана"; break;
                    case "_4HallPage": title = "Выбор зала"; break;
                }
                txtTitle.Text = title;
            }
        }
        private void BtnBackLogin_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                UserData.users = null;
                MainFrame.Navigate(new LoginPage());
            }
        }

        private void dtn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }
    }
}
