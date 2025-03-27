using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace Webrequest
{
    public partial class MainWindow : Window
    {
        private readonly ProductApiClient _apiClient = new ProductApiClient("http://localhost/");

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //await LoadProducts();
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                var products = await _apiClient.GetProductsAsync();
                Products.Clear();
                foreach (var product in products)
                    Products.Add(product);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task GetResponseBody()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost/productcatalogue/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }



        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddProductWindow { Owner = this };
            window.ShowDialog();
        }
    }
}
