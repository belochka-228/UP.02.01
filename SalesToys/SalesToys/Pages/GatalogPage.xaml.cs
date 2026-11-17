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
        public string Source = "";
        public string Sort = "";
        public string Fulter = "";
        public GatalogPage()
        {
            InitializeComponent();
            cmbSupplier.ItemsSource = Core.Context.Suppliers.ToList();
            LoadData();
        }
        public void LoadData()
        {
            var product = Core.Context.Products.Include(p => p.Category).Include(p => p.Suppliers).Include(p => p.Manufacturers).AsQueryable();

            if (!string.IsNullOrWhiteSpace(Source))
            {
                product = product.Where(p => p.Name.ToLower().Contains(Source) || p.Unit.ToLower().Contains(Source) || p.Suppliers.Supplier.ToLower().Contains(Source)
                || p.Manufacturers.Manufacturer.ToLower().Contains(Source) || p.Category.CategoryTovara.ToLower().Contains(Source) ||p.Description.ToLower().Contains(Source));
            }
            if (!string.IsNullOrWhiteSpace(Sort))
            {
                switch (Sort)
                {
                    case "Цене (Возрастание)":
                        product = product.OrderBy(p => p.Price);
                        break;
                    case "Цене (Убывание)":
                        product = product.OrderByDescending(p => p.Price); 
                        break;
                    case "Количеству на складе (Возрастание)":
                        product = product.OrderBy(p => p.Quantity);
                        break;
                    case "Количеству на складе (Убывание)":
                        product = product.OrderByDescending(p => p.Quantity);
                        break;
                    default:
                        Sort = "";
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(Fulter))
            {
                product = product.Where(p => p.Suppliers.Supplier == Fulter);
            }
            lstTovar.ItemsSource = product.ToList();
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

        private void txtSource_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Source = txtSource.Text.Trim().ToLower();
            LoadData();
        }

        private void cmbPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = cmbPrice.SelectedItem as ComboBoxItem;
            string text = comboBoxItem.Content.ToString();
            Sort = text;
            LoadData();
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Suppliers selected = cmbSupplier.SelectedItem as Suppliers;
            Fulter = selected.ToString();
            LoadData();
        }

    }
}
