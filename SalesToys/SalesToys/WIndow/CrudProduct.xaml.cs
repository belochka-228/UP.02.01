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
using SalesToys;

namespace SalesToys.WIndow
{
    /// <summary>
    /// Логика взаимодействия для CrudProduct.xaml
    /// </summary>
    public partial class CrudProduct : Window
    {
        private Products _currentProduct;
        public CrudProduct(Products products)
        {
            InitializeComponent();
            _currentProduct = products ?? new Products();
            LoadData();
            FillFields();
        }
        private void LoadData()
        {
            cmbCategory.ItemsSource = Core.Context.Category.ToList();
            cmbManufacturer.ItemsSource = Core.Context.Manufacturers.ToList();
            cmbSupplier.ItemsSource = Core.Context.Suppliers.ToList();
        }
        private void FillFields()
        {
            if (!string.IsNullOrWhiteSpace(_currentProduct.Artikul))
            {
                txtID.Visibility = Visibility.Visible;
                txtID.Text = _currentProduct.Artikul;
                txtImage.Text = _currentProduct.Image.ToString();
                txtName.Text = _currentProduct.Name;
                cmbCategory.SelectedValue = _currentProduct.IDGategori;
                txtDescription.Text = _currentProduct.Description;
                cmbManufacturer.SelectedValue = _currentProduct.IDManufacturers;
                cmbSupplier.SelectedValue = _currentProduct.IDSuppliers;
                txtPrice.Text = _currentProduct.Price.ToString();
                txtUnit.Text = _currentProduct.Unit.ToString();
                txtQuantity.Text = _currentProduct.Quantity.ToString();
                txtDiscount.Text = _currentProduct.Discount.ToString();
            }
            else
            {
                txtID.Visibility = Visibility.Collapsed;
                txtUnit.Text = "шт.";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(cmbCategory.SelectedValue as string) ||
                string.IsNullOrWhiteSpace(cmbManufacturer.SelectedValue as string) ||
                string.IsNullOrWhiteSpace(cmbSupplier.SelectedValue as string) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtUnit.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                MessageBox.Show("Заполните обязательные поля");
                return; 
            }
            if(!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть числом больше 0");
                return;
            }
            if(!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Количество товара не может быть отрицательным");
                return;
            }
            _currentProduct.Name = txtName.Text.Trim();
            _currentProduct.Description = txtDescription.Text.Trim();
            _currentProduct.Image = string.IsNullOrWhiteSpace(txtImage.Text) ? null : txtImage.Text.Trim();

            _currentProduct.IDGategori = cmbCategory.SelectedIndex;
            _currentProduct.IDManufacturers = cmbManufacturer.SelectedIndex;
            _currentProduct.IDSuppliers = cmbSupplier.SelectedIndex;
        }
    }
}
