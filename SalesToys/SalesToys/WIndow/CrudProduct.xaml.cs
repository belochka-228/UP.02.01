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
using SalesToys;
using Microsoft.Win32;
using System.IO;

namespace SalesToys.WIndow
{
    /// <summary>
    /// Логика взаимодействия для CrudProduct.xaml
    /// </summary>
    public partial class CrudProduct : Window
    {
        private string _selectedNewFilePath = null; //путь к новому файлу
        private string _currentSavedFilePath = null; //путь к текущему сохраненному файлу
        private readonly string _imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image");
        private Products _currentProduct; 
        public CrudProduct(Products products)
        {
            InitializeComponent();
            _currentProduct = products ?? new Products();
            _currentSavedFilePath = _currentProduct.Image;
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
            if (!string.IsNullOrEmpty(_currentSavedFilePath))
            {
                DisplayImage(Path.Combine(_imagesFolder, _currentSavedFilePath));
            }

            if (!string.IsNullOrWhiteSpace(_currentProduct.Artikul))
            {
                spID.Visibility = Visibility.Visible;
                txtID.Text = _currentProduct.IDProduct.ToString();
                txtImage.Text = _currentProduct.Image ?? string.Empty;
                txtArtikul.Text = _currentProduct.Artikul ?? string.Empty;
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
                spID.Visibility = Visibility.Collapsed;
                txtUnit.Text = "шт.";
            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArtikul.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                cmbCategory.SelectedValue == null ||
                cmbManufacturer.SelectedValue == null ||
                cmbSupplier.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtUnit.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть числом больше 0");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Количество товара не может быть отрицательным");
                return;
            }

            if (!double.TryParse(txtDiscount.Text, out double discount) || discount < 0)
            {
                MessageBox.Show("Скидка должна быть числом не меньше 0");
                return;
            }

            if (!string.IsNullOrEmpty(_selectedNewFilePath))
            {
                try
                {
                    if (!Directory.Exists(_imagesFolder))
                    {
                        Directory.CreateDirectory(_imagesFolder);
                    }
                    string fileName = Path.GetFileName(_selectedNewFilePath);
                    string destinationPath = Path.Combine(_imagesFolder, fileName);

                    File.Copy(_selectedNewFilePath, destinationPath, overwrite: true);

                    _currentSavedFilePath = fileName;
                    _selectedNewFilePath = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}");
                    return;
                }
            }

            _currentProduct.Artikul = txtArtikul.Text.Trim();
            _currentProduct.Name = txtName.Text.Trim();
            _currentProduct.Description = txtDescription.Text.Trim();
            _currentProduct.Image = string.IsNullOrWhiteSpace(txtImage.Text) ? null : txtImage.Text.Trim();
            _currentProduct.IDGategori = (int)cmbCategory.SelectedValue;
            _currentProduct.IDManufacturers = (int)cmbManufacturer.SelectedValue;
            _currentProduct.IDSuppliers = (int)cmbSupplier.SelectedValue;
            _currentProduct.Price = (double)price;
            _currentProduct.Unit = txtUnit.Text.Trim();
            _currentProduct.Quantity = quantity;
            _currentProduct.Discount = discount;

            if (_currentProduct.IDProduct == 0)
            {
                Core.Context.Products.Add(_currentProduct);
            }

            try
            {
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
            _selectedNewFilePath = null;
            Close();
        }

        private void BtnDowlound_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Выберите изображение",
                Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };
            if(openFileDialog.ShowDialog() ==true)
            {
                _selectedNewFilePath = openFileDialog.FileName;
                txtImage.Text = Path.GetFileName(_selectedNewFilePath);
                DisplayImage(_selectedNewFilePath);
            }
        }
        private void DisplayImage(string FileName)
        {
            if(string.IsNullOrWhiteSpace(FileName) || !File.Exists(FileName))
            {
                ImgPreview.Source = null;
                return;
            }
            BitmapImage bitmap = new BitmapImage(); //контейнер для изображения
            bitmap.BeginInit(); //настраиваем без загрузки
            bitmap.UriSource = new Uri(FileName); //точный адрес изображения
            bitmap.CacheOption = BitmapCacheOption.OnLoad; //загрузка в кеш
            bitmap.EndInit(); // загрузка

            ImgPreview.Source = bitmap; //привязка
        }
    }
}
