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
        private List<Product> _products = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _products = await GetProductsAsync("https://pmarcelis.mid-ica.nl/products/");

            // Bind our DataGrid to the product list
            DataContext = _products;
        }

        // Handle button click to create a new product
        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation or parse checks would go here
            decimal price;
            if (!decimal.TryParse(NewProductPrice.Text, out price))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            var newProduct = new Product
            {
                Name = NewProductName.Text,
                Price = price,
                ShortDescription = NewProductDescription.Text
            };

            var created = await PostProductAsync("https://pmarcelis.mid-ica.nl/products/", newProduct);
            if (created != null)
            {
                _products.Add(created);
                ProductsDataGrid.Items.Refresh();

                // Clear the input fields
                NewProductName.Text = string.Empty;
                NewProductPrice.Text = string.Empty;
                NewProductDescription.Text = string.Empty;
            }
        }

        // GET the list of products
        private async Task<List<Product>> GetProductsAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Could not retrieve products");
                    return new List<Product>();
                }

                string json = await response.Content.ReadAsStringAsync();
                var wrapper = JsonConvert.DeserializeObject<ApiResponse<List<Product>>>(json);

                // If your API just returns a List<Product> directly, skip the 'ApiResponse' class
                return wrapper?.Data ?? new List<Product>();
            }
        }

        // POST a new product
        private async Task<Product> PostProductAsync(string url, Product product)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error creating product: {response.StatusCode}");
                    return null;
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                // If your API returns the created product, deserialize it:
                var returnData = JsonConvert.DeserializeObject<ApiResponse<Product>>(responseBody);
                return returnData?.Data ?? new Product();
            }
        }
    }
}
