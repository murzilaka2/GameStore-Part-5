using GameStore.Interfaces;
using GameStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IProduct _products;
        private readonly IOrder _orders;

        public OrdersController(IProduct products, IOrder orders)
        {
            _products = products;
            _orders = orders;
        }
        public IActionResult Index()
        {
            return View(_orders.GetAllOrders());
        }
        public IActionResult EditOrder(int id)
        {
            var products = _products.GetAllProducts();
            Order order = id == 0 ? new Order() : _orders.GetOrder(id);
            IDictionary<int, OrderLine> lineMaps = order.Lines?.ToDictionary(e => e.ProductId) ?? new Dictionary<int, OrderLine>();
            ViewBag.Lines = products.Select(e => lineMaps.ContainsKey(e.Id) ? lineMaps[e.Id] : new OrderLine
            {
                Product = e,
                ProductId = e.Id,
                Quantity = 0
            });
            return View(order);
        }
        [HttpPost]
        public IActionResult AddOrUpdateOrder(Order order)
        {
            order.Lines = order.Lines.Where(e => e.Id > 0 || (e.Id == 0 && e.Quantity > 0)).ToArray();
            if (order.Id == 0)
            {
                _orders.AddOrder(order);
            }
            else
            {
                _orders.UpdateOrder(order);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteOrder(Order order)
        {
            _orders.DeleteOrder(order);
            return RedirectToAction(nameof(Index));
        }
    }

}
