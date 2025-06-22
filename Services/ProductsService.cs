using HongDucFashion.Models;
using Microsoft.AspNetCore.Components;

namespace HongDucFashion.Services
{
    public class ProductsService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;

        private string ApiUrl => new Uri(new Uri(_navigationManager.BaseUri), "api/products").ToString();

        public ProductsService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public async Task<List<Product>?> GetAllProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>(ApiUrl);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"{ApiUrl}/{id}");
        }

        public async Task<HttpResponseMessage> CreateProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, product);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateProductAsync(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/{id}", product);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            return response;
        }
    }
}
