using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Webrequest
{
    public partial class AddProductWindow : Window
    {
        private readonly ProductApiClient _apiClient = new ProductApiClient("http://localhost/");

        public AddProductWindow()
        {
            InitializeComponent();
        }

        private async void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Please enter a name.");
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                {
                    MessageBox.Show("Please enter a valid decimal price.");
                    return;
                }

                var productType = (ProductType)cmbProductType.SelectedValue;

                var newProduct = new Product(
                    txtName.Text,
                    price,
                    txtDescription.Text,
                    productType
                );

                var addedProduct = await _apiClient.AddProductAsync(newProduct);
                MessageBox.Show($"Product added: {addedProduct.Name}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

               

                if (Owner is MainWindow main)
                {
                    main.Products.Add(addedProduct);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Ensures only numeric (decimal) input is allowed for Price field
        private void txtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
