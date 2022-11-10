using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;
using System.Net.Http.Json;

namespace OnlineShop.Web.Services
{
    public class ClientProductService : IClientProductService
    {
        private readonly HttpClient httpClient;

        public ClientProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                var productsDto = await httpClient
                    .GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
                return productsDto;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
