using MS.Web.Models;
using MS.Web.Service.IService;
using MS.Web.Utility;

namespace MS.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/applycoupon"
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/emailshoppingcart"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/getcart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDetailsId,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/removecart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/cartupsert"
            });
        }
    }
}
