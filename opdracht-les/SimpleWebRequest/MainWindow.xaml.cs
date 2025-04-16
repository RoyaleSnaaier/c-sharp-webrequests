using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            DataContext = _products;
        }

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(NewProductPrice.Text, out decimal price))
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
                ClearFormFields();
            }
        }

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
                return wrapper?.Data ?? new List<Product>();
            }
        }

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
                var returnData = JsonConvert.DeserializeObject<ApiResponse<Product>>(responseBody);
                return returnData?.Data ?? new Product();
            }
        }

        private async Task<bool> UpdateProductAsync(Product product)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://pmarcelis.mid-ica.nl/products/?id={product.Id}";
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product successfully updated inasn e wow jij bent5 zooo cooolllll");
                    return true;
                }
                else
                {
                    MessageBox.Show("Product could not be updated.");
                    return false;
                }
            }
        }

        private async Task<bool> DeleteProductAsync(Product product)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://pmarcelis.mid-ica.nl/products/?id={product.Id}";
                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product successfully deleted yay yipi wowwowoowwwowowow yayyyyy");
                    return true;
                }
                else
                {
                    MessageBox.Show("Product could not be deleted u mf.");
                    return false;
                }
            }
        }

        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFormFields();
            ToggleButtons(true);
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                PopulateFormFields(selectedProduct);
                ToggleButtons(false);
            }
        }

        private async void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                selectedProduct.Name = NewProductName.Text;
                selectedProduct.Price = decimal.Parse(NewProductPrice.Text);
                selectedProduct.ShortDescription = NewProductDescription.Text;
                await UpdateProductAsync(selectedProduct);
            }
        }

        private async void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                await DeleteProductAsync(selectedProduct);
                _products.Remove(selectedProduct);
                ProductsDataGrid.Items.Refresh();
            }
        }

        private void ClearFormFields()
        {
            NewProductName.Text = string.Empty;
            NewProductPrice.Text = string.Empty;
            NewProductDescription.Text = string.Empty;
        }

        private void ToggleButtons(bool showAddButton)
        {
            AddProductButton.Visibility = showAddButton ? Visibility.Visible : Visibility.Collapsed;
            UpdateProductButton.Visibility = showAddButton ? Visibility.Collapsed : Visibility.Visible;
            CancelEditButton.Visibility = showAddButton ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PopulateFormFields(Product product)
        {
            NewProductName.Text = product.Name;
            NewProductPrice.Text = product.Price.ToString();
            NewProductDescription.Text = product.ShortDescription;
        }

        private void ProductsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
