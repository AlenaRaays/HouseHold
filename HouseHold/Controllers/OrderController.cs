// Controllers/OrderController.cs
using HouseHold.Models;
using HouseHold.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseHold.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly ILogger<OrderController> _logger;

        public OrderController(DataBaseContext context, ILogger<OrderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Страница оформления заказа
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            // Получаем корзину пользователя
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.user_id == userId);

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Корзина пуста";
                return RedirectToAction("Index", "Cart");
            }

            // Получаем пользователя
            var user = await _context.users
                .FirstOrDefaultAsync(u => u.user_id == userId);

            // Проверяем наличие товаров
            var outOfStockItems = new List<string>();
            var insufficientItems = new List<CartItem>();

            foreach (var item in cart.Items)
            {
                var product = item.Product;

                if (product.amount <= 0)
                {
                    outOfStockItems.Add(product.name);
                }
                else if (product.amount < item.quantity)
                {
                    insufficientItems.Add(item);
                }
            }

            if (outOfStockItems.Any())
            {
                TempData["Error"] = $"Товары закончились: {string.Join(", ", outOfStockItems)}";
                return RedirectToAction("Index", "Cart");
            }

            if (insufficientItems.Any())
            {
                foreach (var item in insufficientItems)
                {
                    TempData["Warning"] = $"Товар \"{item.Product.name}\" доступен в количестве {item.Product.amount} шт. Количество в корзине изменено.";
                    item.quantity = item.Product.amount;
                }
                await _context.SaveChangesAsync();
            }

            // Рассчитываем суммы
            double subTotal = cart.Items.Sum(i => i.Product.price * i.quantity);
            double discount = user?.discount ?? 0;
            double discountAmount = subTotal * discount / 100;

            // Получаем методы доставки и оплаты
            var deliveryMethods = await _context.deliveryMethods.ToListAsync();
            var paymentMethods = await _context.paymentMethods.ToListAsync();

            var viewModel = new CheckoutViewModel
            {
                Items = cart.Items.Select(i => new CartItemViewModel
                {
                    ProductId = i.product_id,
                    Name = i.Product.name,
                    Price = i.Product.price,
                    Quantity = i.quantity
                }).ToList(),
                SubTotal = subTotal,
                Discount = discountAmount,
                DeliveryMethods = deliveryMethods,
                PaymentMethods = paymentMethods
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            if (!ModelState.IsValid)
            {
                model.DeliveryMethods = await _context.deliveryMethods.ToListAsync();
                model.PaymentMethods = await _context.paymentMethods.ToListAsync();
                return View(model);
            }

            // Начинаем транзакцию
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Получаем корзину с товарами
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.user_id == userId);

                if (cart == null || !cart.Items.Any())
                {
                    TempData["Error"] = "Корзина пуста";
                    return RedirectToAction("Index", "Cart");
                }

                // Получаем пользователя
                var user = await _context.users
                    .FirstOrDefaultAsync(u => u.user_id == userId);

                // Получаем стоимость доставки
                var deliveryMethod = await _context.deliveryMethods
                    .FirstOrDefaultAsync(d => d.delivery_method_id == model.DeliveryMethodId);

                // Финальная проверка наличия товаров и обновление количества
                foreach (var item in cart.Items)
                {
                    var product = item.Product;

                    // Проверяем, что товар существует и видимый
                    if (product == null || !product.is_visible)
                    {
                        throw new Exception($"Товар \"{item.Product?.name ?? "Неизвестный"}\" недоступен");
                    }

                    // Проверяем достаточное количество
                    if (product.amount < item.quantity)
                    {
                        throw new Exception($"Товар \"{product.name}\" доступен только в количестве {product.amount} шт.");
                    }

                    // СПИСЫВАЕМ ТОВАРЫ СО СКЛАДА
                    product.amount -= item.quantity;
                    _context.products.Update(product);
                }

                // Рассчитываем итоговую сумму
                double subTotal = cart.Items.Sum(i => i.Product.price * i.quantity);
                double discountPercent = user?.discount ?? 0;
                double discountAmount = subTotal * discountPercent / 100;
                double deliveryCost = deliveryMethod?.cost ?? 0;
                double totalAmount = subTotal - discountAmount + deliveryCost;

                string orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{userId}-{new Random().Next(1000, 9999)}";

                var order = new Orders
                {
                    order_number = orderNumber,
                    user_id = userId.Value,
                    status_id = 1, // Новый заказ (убедитесь что такой статус есть)
                    delivery_method_id = model.DeliveryMethodId,
                    payment_method_id = model.PaymentMethodId,
                    created_date = DateTime.Now,
                    total_amount = (int)totalAmount,
                    delivery_address = model.DeliveryAddress,
                    order_comment = string.IsNullOrWhiteSpace(model.OrderComment) ? "" : model.OrderComment,
                    discount_at_order = discountPercent
                };

                _context.orders.Add(order);
                await _context.SaveChangesAsync(); // Сохраняем чтобы получить order_id

                // Создаем позиции заказа
                foreach (var item in cart.Items)
                {
                    var orderItem = new OrderItem
                    {
                        order_id = order.order_id,
                        product_id = item.product_id,
                        quantity = item.quantity,
                        price_at_order = (float)item.Product.price
                    };
                    _context.orderItems.Add(orderItem);
                }

                // Очищаем корзину
                _context.CartItems.RemoveRange(cart.Items);

                // Сохраняем все изменения
                await _context.SaveChangesAsync();

                // Фиксируем транзакцию
                await transaction.CommitAsync();

                TempData["Success"] = $"Заказ №{orderNumber} успешно оформлен!";

                // Сохраняем ID заказа для страницы успеха
                HttpContext.Session.SetInt32("LastOrderId", order.order_id);

                return RedirectToAction("Success", new { orderId = order.order_id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Ошибка при оформлении заказа");
                TempData["Error"] = ex.Message;

                // Перезагружаем данные для формы
                model.DeliveryMethods = await _context.deliveryMethods.ToListAsync();
                model.PaymentMethods = await _context.paymentMethods.ToListAsync();

                return View(model);
            }
        }

        // Страница успешного оформления заказа
        [HttpGet]
        public async Task<IActionResult> Success(int orderId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            var order = await _context.orders
                .Include(o => o.orderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.deliveryMethod)
                .Include(o => o.paymentMethod)
                .Include(o => o.status)
                .FirstOrDefaultAsync(o => o.order_id == orderId && o.user_id == userId);

            if (order == null)
                return RedirectToAction("Index", "MainShop");

            return View(order);
        }

        // История заказов пользователя
        [HttpGet]
        public async Task<IActionResult> History()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            var orders = await _context.orders
                .Include(o => o.status)
                .Include(o => o.orderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.user_id == userId)
                .OrderByDescending(o => o.created_date)
                .ToListAsync();

            return View(orders);
        }

        // Детали заказа
        [HttpGet]
        public async Task<IActionResult> Details(int orderId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            var order = await _context.orders
                .Include(o => o.orderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.deliveryMethod)
                .Include(o => o.paymentMethod)
                .Include(o => o.status)
                .FirstOrDefaultAsync(o => o.order_id == orderId && o.user_id == userId);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}