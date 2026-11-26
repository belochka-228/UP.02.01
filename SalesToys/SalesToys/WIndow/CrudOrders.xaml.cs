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

namespace SalesToys.WIndow
{
    /// <summary>
    /// Логика взаимодействия для CrudOrders.xaml
    /// </summary>
    public partial class CrudOrders : Window
    {
        private Orders _currentOrder;
        private OrderProduct _currentOrderProduct;

        public CrudOrders(Orders order)
        {
            InitializeComponent();
            LoadData();

            if (order != null)
            {
                _currentOrder = order;
                _currentOrder = Core.Context.Orders.FirstOrDefault(o => o.IDOrder == order.IDOrder) ?? order;
            }
            else
            {
                _currentOrder = new Orders
                {
                    OrderDate = DateTime.Today,
                    DeliveryDate = DateTime.Today.AddDays(3)
                };
            }

            FillFields();
        }

        private void LoadData()
        {
            cmbArtikul.ItemsSource = Core.Context.Products.ToList();
            cmbStatus.ItemsSource = Core.Context.Status.ToList();
            cmbPVZ.ItemsSource = Core.Context.PVZ.ToList();
        }

        private void FillFields()
        {
            if (_currentOrderProduct != null)
            {
                cmbArtikul.SelectedValue = _currentOrderProduct.IDProduct;
            }

            if (_currentOrder.IDStatus > 0)
            {
                cmbStatus.SelectedValue = _currentOrder.IDStatus;
            }

            if (_currentOrder.AddressPoint > 0)
            {
                cmbPVZ.SelectedValue = _currentOrder.AddressPoint;
            }

            dpOrderDate.SelectedDate = _currentOrder.OrderDate == default(DateTime)
                ? DateTime.Today
                : _currentOrder.OrderDate;

            dpDeliveryDate.SelectedDate = _currentOrder.DeliveryDate == default(DateTime)
                ? DateTime.Today.AddDays(3)
                : _currentOrder.DeliveryDate;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbArtikul.SelectedItem == null ||
                cmbStatus.SelectedItem == null ||
                cmbPVZ.SelectedItem == null ||
                !dpOrderDate.SelectedDate.HasValue ||
                !dpDeliveryDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (dpDeliveryDate.SelectedDate.Value.Date < dpOrderDate.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата выдачи не может быть раньше даты заказа");
                return;
            }

            try
            {
                int productId = (int)cmbArtikul.SelectedValue;

                _currentOrder.IDStatus = (int)cmbStatus.SelectedValue;
                _currentOrder.AddressPoint = (int)cmbPVZ.SelectedValue;
                _currentOrder.OrderDate = dpOrderDate.SelectedDate.Value;
                _currentOrder.DeliveryDate = dpDeliveryDate.SelectedDate.Value;

                if (_currentOrder.IDOrder == 0)
                {
                    _currentOrder.IDUsers = UserData.users != null ? UserData.users.ID : 1;
                    _currentOrder.Kod = new Random().Next(100, 999);

                    Core.Context.Orders.Add(_currentOrder);

                    _currentOrderProduct = new OrderProduct
                    {
                        Orders = _currentOrder,
                        IDProduct = productId,
                        Quantity = 1
                    };
                    Core.Context.OrderProduct.Add(_currentOrderProduct);
                }
                else if (_currentOrderProduct != null)
                {
                    _currentOrderProduct.IDProduct = productId;
                }
                else
                {
                    _currentOrderProduct = new OrderProduct
                    {
                        IDOrder = _currentOrder.IDOrder,
                        IDProduct = productId,
                        Quantity = 1
                    };
                    Core.Context.OrderProduct.Add(_currentOrderProduct);
                }

                Core.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
