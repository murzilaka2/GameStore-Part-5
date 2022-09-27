using GameStore.Interfaces;
using GameStore.Models.Pages;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProduct _products;
        private readonly ICategory _categories;

        public StoreController(IProduct products, ICategory categories)
        {
            _products = products;
            _categories = categories;
        }
        public IActionResult Index([FromQuery(Name = "options")] QueryOptions productOptions, QueryOptions catOptions, int category)
        {
            ViewBag.Categories = _categories.GetCategories(catOptions);
            ViewBag.SelectedCategory = category;
            return View(_products.GetProducts(productOptions, category));
        }
    }
}
