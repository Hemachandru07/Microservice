using MS.Web.Models;
using MS.Web.Utility;

namespace MS.Web.Service.IService
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = productDto,
                Url = StaticDetails.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> DeleteProductsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> GetProductAsync(string productName)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/product/GetByProduct/" + productName
            });
        }

        public async Task<ResponseDto?> GetroductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = productDto,
                Url = StaticDetails.ProductAPIBase + "/api/product"
            });
        }
    }
}
