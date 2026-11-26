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
            lstOrders.ItemsSource = Core.Context.OrderProduct.Include(op => op.Products).Include(op => op.Orders.PVZ)
                            .Include(op => op.Orders.Status).ToList();
        }
        private void lstOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserData.users == null || UserData.users.IDRoles != 1)
                return;
            var selectedTovar = lstOrders.SelectedItem as OrderProduct;
            if (selectedTovar == null || selectedTovar.Orders == null)
                return;
            var crudOrder = new CrudOrders(selectedTovar.Orders);
            crudOrder.ShowDialog();
            LoadData();
        }

        private void ButtonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var crudOrder = new CrudOrders(null);
            crudOrder.ShowDialog();

            LoadData();
        }

        private void ButtonDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!(lstOrders.SelectedItem is OrderProduct selectedItem) || selectedItem.Orders == null)
            {
                MessageBox.Show("Сначала выберите заказ!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var order = selectedItem.Orders;

            if (MessageBox.Show($"Вы уверены в удалении заказа №{order.IDOrder}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var orderProducts = Core.Context.OrderProduct.Where(op => op.IDOrder == order.IDOrder).ToList();
                    Core.Context.OrderProduct.RemoveRange(orderProducts);
                    Core.Context.Orders.Remove(order);

                    Core.Context.SaveChanges();
                    MessageBox.Show($"Заказ {order.IDOrder} успешно удалён");
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
    }
}
