using HouseHold.Helpers;
using HouseHold.Models;
using HouseHold.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly DataBaseContext _context;

    public CartController(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        if (userId == null)
            return RedirectToAction("Index", "Authorize");

        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.user_id == userId);

        var vm = new CartViewModel();

        if (cart != null)
        {
            vm.Items = cart.Items.Select(i => new CartItemViewModel
            {
                ProductId = i.product_id,
                Name = i.Product.name,
                Price = i.Product.price,
                Quantity = i.quantity
            }).ToList();
        }

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        if (userId == null)
        {
            TempData["Error"] = "Для добавления в корзину необходимо авторизоваться";
            return RedirectToAction("Index", "Authorize");
        }

        // Получаем товар для проверки наличия
        var product = await _context.products
            .FirstOrDefaultAsync(p => p.product_id == productId);

        if (product == null)
        {
            TempData["Error"] = "Товар не найден";
            return RedirectToAction("Index", "MainShop");
        }

        // Проверяем, видимый ли товар
        if (!product.is_visible)
        {
            TempData["Error"] = "Товар недоступен для заказа";
            return RedirectToAction("Index", "MainShop");
        }

        // Получаем или создаем корзину пользователя
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.user_id == userId);

        if (cart == null)
        {
            cart = new Cart { user_id = userId.Value };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Проверяем, есть ли уже такой товар в корзине
        var cartItem = cart.Items?.FirstOrDefault(i => i.product_id == productId);
        int currentQuantityInCart = cartItem?.quantity ?? 0;
        int requestedTotalQuantity = currentQuantityInCart + quantity;

        // Проверяем, достаточно ли товара на складе
        if (product.amount < requestedTotalQuantity)
        {
            int available = product.amount - currentQuantityInCart;

            if (available <= 0)
            {
                TempData["Error"] = $"Товар \"{product.name}\" закончился на складе";
            }
            else
            {
                TempData["Error"] = $"Доступно только {available} шт. товара \"{product.name}\"";
            }

            // Сохраняем в TempData и сразу помечаем как использованное
            TempData["ShowError"] = true;

            return RedirectToAction("Index", "MainShop");
        }

        // Добавляем или обновляем товар в корзине
        if (cartItem != null)
        {
            cartItem.quantity = requestedTotalQuantity;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                cart_id = cart.cart_id,
                product_id = productId,
                quantity = quantity
            });
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Товар \"{product.name}\" добавлен в корзину";

        return RedirectToAction("Index", "MainShop");
    }
    [HttpPost]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        if (userId == null)
            return RedirectToAction("Index", "Authorize");

        if (quantity < 1)
            quantity = 1;

        // Получаем товар для проверки
        var product = await _context.products
            .FirstOrDefaultAsync(p => p.product_id == productId);

        if (product == null)
        {
            TempData["Error"] = "Товар не найден";
            return RedirectToAction("Index");
        }

        // Проверяем наличие на складе
        if (product.amount < quantity)
        {
            TempData["Error"] = $"Доступно только {product.amount} шт. товара \"{product.name}\"";
            return RedirectToAction("Index");
        }

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.user_id == userId);

        var item = cart?.Items.FirstOrDefault(x => x.product_id == productId);

        if (item != null)
        {
            item.quantity = quantity;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Количество обновлено";
        }

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        if (userId == null)
            return RedirectToAction("Index", "Authorize");

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.user_id == userId);

        if (cart != null && cart.Items.Any())
        {
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Корзина очищена";
        }

        return RedirectToAction("Index");
    }


}