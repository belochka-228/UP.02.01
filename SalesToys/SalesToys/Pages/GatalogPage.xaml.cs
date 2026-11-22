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
using static System.Net.Mime.MediaTypeNames;

namespace SalesToys.Pages
{
    /// <summary>
    /// Логика взаимодействия для GatalogPage.xaml
    /// </summary>
    public partial class GatalogPage : Page
    {
        private List<Products> _allProducts = new List<Products>();
        public string Source = "";
        public string Sort = "";
        public string Fulter = "";
        
        public GatalogPage()
        {
            InitializeComponent();
            LoadData();
            UserRole();
        }
        private void UserRole()
        {
            if (UserData.users == null)
            {
                FilterPanel.Visibility = Visibility.Collapsed;
                return;
            }

            switch (UserData.users.IDRoles)
            {
                case 1:
                    FilterPanel.Visibility = Visibility.Visible;
                    TovarPanel.Visibility = Visibility.Visible;
                    break;
                case 2:
                    FilterPanel.Visibility = Visibility.Visible;
                    TovarPanel.Visibility = Visibility.Collapsed;
                    break;
                default:
                    FilterPanel.Visibility = Visibility.Collapsed;
                    TovarPanel.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        public void LoadData()
        {
            _allProducts = Core.Context.Products.Include(p => p.Category).Include(p => p.Suppliers).Include(p => p.Manufacturers).ToList();
            
            var SupplerName = Core.Context.Suppliers.Select(s => s.Supplier).Distinct().OrderBy(s => s).ToList();
            SupplerName.Insert(0, "Все поставщики");
            cmbSupplier.ItemsSource = SupplerName;
            cmbSupplier.SelectedIndex = 0;

            ApplyFilter();
        }
        public void ApplyFilter()
        {
            IEnumerable<Products> filteredList = _allProducts;

            if (!string.IsNullOrWhiteSpace(Source))
            {
                string[] SearceWords = Source.Split(new[] { ' '}, StringSplitOptions.RemoveEmptyEntries);
                filteredList = filteredList.Where(p =>
                {
                    string combinedText = $"{p.Name} {p.Unit} {p.Suppliers.Supplier} {p.Manufacturers.Manufacturer} ".ToLower() +
                    $"{p.Category.CategoryTovara} {p.Description} ".ToLower();
                    return SearceWords.All(word => combinedText.Contains(word));
                });
                
                //product = product.Where(p => p.Name.ToLower().Contains(Source) || p.Unit.ToLower().Contains(Source) || p.Suppliers.Supplier.ToLower().Contains(Source)
                //|| p.Manufacturers.Manufacturer.ToLower().Contains(Source) || p.Category.CategoryTovara.ToLower().Contains(Source) ||p.Description.ToLower().Contains(Source));
            }
            if (!string.IsNullOrWhiteSpace(Sort))
            {
                switch (Sort)
                {
                    case "Цене (Возрастание)":
                        filteredList = filteredList.OrderBy(p => p.Price);
                        break;
                    case "Цене (Убывание)":
                        filteredList = filteredList.OrderByDescending(p => p.Price); 
                        break;
                    case "Количеству на складе (Возрастание)":
                        filteredList = filteredList.OrderBy(p => p.Quantity);
                        break;
                    case "Количеству на складе (Убывание)":
                        filteredList = filteredList.OrderByDescending(p => p.Quantity);
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(Fulter))
            {
                filteredList = filteredList.Where(p => p.Suppliers.Supplier == Fulter);
            }
            lstTovar.ItemsSource = filteredList.ToList();
        }

        private void ItemBorder_Loaded(object sender, RoutedEventArgs e)
        {
            var border = sender as Border;
            var p = border.DataContext as Products;

            if(p.Quantity == 0)
            {
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87CEEB"));
            }
            else if (p.Discount > 17)
            {
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDEAD"));
            }
            else
            {
                border.Background = Brushes.White;
            }
        }

        private void OldPrice_Loaded(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBlock;
            var p = tb.DataContext as Products;

            tb.Visibility = p.Discount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void FinalPrice_Loaded(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBlock;
            var p = tb.DataContext as Products;

            decimal final = Math.Round((decimal)p.Price * (1 - (decimal)p.Discount / 100), 2);
            tb.Text = final.ToString();
        }

        private void txtSource_TextChanged(object sender, RoutedEventArgs e)
        {
            Source = txtSource.Text? .Trim().ToLower();
            ApplyFilter();
        }

        private void cmbPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = cmbPrice.SelectedItem as ComboBoxItem;
            string text = comboBoxItem.Content.ToString();
            Sort = text;
            ApplyFilter();
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = cmbSupplier.SelectedItem as string;
            if( selected == "Все поставщики")
            {
                Fulter = "";
            }
            else
            {
                Fulter = selected ?? "";
            }
            ApplyFilter();
        }

        private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var crudProduct = new CrudProduct(null);
            crudProduct.ShowDialog();

            LoadData();
        }

        private void ButtonDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var BorderItem = sender as Border;
            var product = BorderItem.DataContext as Products;

            if(product != null)
            {
                if(MessageBox.Show("Вы уверены в удалении товара", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Core.Context.Products.Remove(product);
                    try
                    {
                        Core.Context.SaveChanges();
                        MessageBox.Show($"Товар {product.Name} успешно удален");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            }
        }

        private void lstTovar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTovar = lstTovar.SelectedItem as Products;
            if(selectedTovar != null)
            {
                var crudProduct = new CrudProduct(selectedTovar);
                crudProduct.ShowDialog();
                LoadData();
            }
        }
    }
}
