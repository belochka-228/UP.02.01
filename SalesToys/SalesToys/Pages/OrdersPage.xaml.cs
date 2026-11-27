using SalesToys;
using SalesToys.WIndow;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            LoadData();
            UserRole();
        }
        private void UserRole()
        {
            if(UserData.users.IDRoles == 1)
            {
               TovarPanel.Visibility = Visibility.Visible;
            }
            else
            {
                TovarPanel.Visibility = Visibility.Collapsed;
            }
        }
        private void LoadData()
        {
           // lstOrders.ItemsSource = Core.Context.OrderProduct.Include(op => op.Products).Include(op => op.Orders.PVZ).Include(op => op.Orders.Status).ToList();

            lstOrders.ItemsSource = Core.Context.Orders.ToList();
        }

        private void ButtonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var crudOrder = new CrudOrders(null);
            crudOrder.ShowDialog();

            LoadData();
        }

        private void ButtonDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lstOrders.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите заказ!", "Ошибка");
                return;
            }

             var selectedOrder = lstOrders.SelectedItem as Orders;

            if (MessageBox.Show($"Вы уверены в удалении заказа №{selectedOrder.IDOrder}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var orderProducts = Core.Context.OrderProduct.Where(op => op.IDOrder == selectedOrder.IDOrder).ToList();
                    //Core.Context.OrderProduct.RemoveRange(orderProducts);
                    Core.Context.Orders.Remove(selectedOrder);

                    Core.Context.SaveChanges();
                    MessageBox.Show($"Заказ {selectedOrder.IDOrder} успешно удалён");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void lstOrders_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (UserData.users == null || UserData.users.IDRoles != 1)
                return;
            var selectedTovar = lstOrders.SelectedItem as Orders;
            if (selectedTovar == null || selectedTovar == null)
                return;
            var crudOrder = new CrudOrders(selectedTovar);
            crudOrder.ShowDialog();
            LoadData();
        }
    }
}
