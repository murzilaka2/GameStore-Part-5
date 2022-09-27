using GameStore.Interfaces;
using GameStore.Models;
using GameStore.Models.Pages;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _products;
        private readonly ICategory _categories;

        public HomeController(IProduct products, ICategory categories)
        {
            _products = products;
            _categories = categories;
        }
        [HttpGet]
        public IActionResult Index(QueryOptions options)
        {
            return View(_products.GetProducts(options));
        }
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            ViewBag.Categories = _categories.GetAllCategories();
            return View(id == 0 ? new Product() : _products.GetProduct(id));
        }
        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (product.Id == 0)
            {
                _products.AddProduct(product);
            }
            else
            {
                _products.UpdateProduct(product);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {
            _products.DeleteProduct(product);
            return RedirectToAction(nameof(Index));
        }
    }

}
