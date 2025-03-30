using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleWebRequest
{
    public partial class MainWindow : Window
    {
        private Product product;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var product = await GetProductsAsync("https://pmarcelis.mid-ica.nl/products/simpleProduct.php");

            // Bind our DataGrid to the product list
            DataContext = product;
        }

        // GET the list of products 
        private async Task<Product> GetProductsAsync(string url)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            var product = JsonConvert.DeserializeObject<Product>(json);

            return product;
        }
    }
}