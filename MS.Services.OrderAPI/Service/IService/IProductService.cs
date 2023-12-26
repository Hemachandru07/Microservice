using MS.Services.OrderAPI.Models.Dto;

namespace MS.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
