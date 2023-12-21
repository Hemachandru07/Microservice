using MS.Web.Models;

namespace MS.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetroductByIdAsync(int id);
        Task<ResponseDto?> GetProductAsync(string productName);
        Task<ResponseDto?> CreateProductsAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto);
        Task<ResponseDto?> DeleteProductsAsync(int id);

    }
}
