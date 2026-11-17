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
using System.Data.Entity;

namespace SalesToys.Pages
{
    /// <summary>
    /// Логика взаимодействия для GatalogPage.xaml
    /// </summary>
    public partial class GatalogPage : Page
    {
        public GatalogPage()
        {
            InitializeComponent();
            lstTovar.ItemsSource = Core.Context.Products
                .Include(p => p.Category)
                .Include(p => p.Suppliers)
                .Include(p => p.Manufacturers)
                .ToList();
        }
    }
}
