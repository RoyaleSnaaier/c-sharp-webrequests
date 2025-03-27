using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SimpleWebRequest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Product retrievedProduct;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.Write("Getting product");
             retrievedProduct = await HaalProductOpAsync("http://localhost/productcatalogue/simpleProduct.php");
            DataContext = retrievedProduct;
        }


public async Task<Product> HaalProductOpAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Product product = JsonConvert.DeserializeObject<Product>(json);
                    return product;
                } else
                {
                    Console.Write("Could not find anything");
                }
                return null;
            }
        }

    }
}
