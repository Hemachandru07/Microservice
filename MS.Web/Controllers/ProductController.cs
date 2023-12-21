using Microsoft.AspNetCore.Mvc;
using MS.Web.Models;
using MS.Web.Service.IService;
using Newtonsoft.Json;

namespace MS.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? products = new();

            ResponseDto? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductsAsync(productDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product added successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(productDto);
        }

		public async Task<IActionResult> ProductEdit(int productId)
		{
            ResponseDto? response = await _productService.GetroductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(productDto);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

		[HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _productService.UpdateProductsAsync(productDto);

				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Product edited successfully";
					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}
			return View(productDto);
		}
		public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? response = await _productService.GetroductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(productDto);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteProductsAsync(productDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }
    }
}
