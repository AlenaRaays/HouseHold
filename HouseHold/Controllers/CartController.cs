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
    public async Task<IActionResult> Add(int productId)
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        if (userId == null)
            return RedirectToAction("Index", "Authorize");

        var cart = await _context.Carts
            .FirstOrDefaultAsync(c => c.user_id == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                user_id = userId.Value
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var item = await _context.CartItems
            .FirstOrDefaultAsync(x =>
                x.cart_id == cart.cart_id &&
                x.product_id == productId);

        if (item != null)
        {
            item.quantity++;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                cart_id = cart.cart_id,
                product_id = productId,
                quantity = 1
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.user_id == userId);

        var item = cart?.Items.FirstOrDefault(x => x.product_id == productId);

        if (item != null)
        {
            item.quantity = quantity;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int productId)
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.user_id == userId);

        var item = cart?.Items.FirstOrDefault(x => x.product_id == productId);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            _context.Entry(cart).Reload();
        }

        return RedirectToAction("Index");
    }
}