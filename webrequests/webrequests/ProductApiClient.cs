using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows;

namespace Webrequest
{
    public class ProductApiClient
    {
        private readonly RestClient _client;

        public ProductApiClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var request = new RestRequest("productcatalogue", Method.Get);
            var response = await _client.ExecuteAsync<ApiResponse<List<Product>>>(request);

            if (response.IsSuccessful)
            {
                var products = new List<Product>();
                foreach (var product in response.Data.Data)
                {
                    products.Add(new Product(product.Name, product.Price, product.ShortDescription, (ProductType)product.ProductType));
                }
                return products;
            }

            throw new Exception(response.ErrorMessage ?? "Failed to get products.");
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var request = new RestRequest("productcatalogue/", Method.Post);

            var jsonBody = System.Text.Json.JsonSerializer.Serialize(product);

            // Display JSON clearly before sending
            MessageBox.Show(jsonBody, "JSON being sent");

            request.AddStringBody(jsonBody, ContentType.Json);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                // TEMPORARY DEBUGGING CODE
                MessageBox.Show(response.Content, "RAW JSON RESPONSE");

                // Inspect the content visually:
                var data = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<Product>>(response.Content);

                var retrievedProduct = data.Data;
                return new Product(retrievedProduct.Name, retrievedProduct.Price, retrievedProduct.ShortDescription, (ProductType)retrievedProduct.ProductType);
            }

            throw new Exception(response.ErrorMessage ?? "Failed to add product.");
        }
    }
}
