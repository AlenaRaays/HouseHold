using HouseHold.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true; // Защита от XSS
    options.Cookie.IsEssential = true; // Сессия работает даже без согласия на cookies
    options.Cookie.SameSite = SameSiteMode.Lax; // Защита от CSRF
});

// Добавьте HTTP контекст (опционально, но полезно)
builder.Services.AddHttpContextAccessor();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataBaseContext>(options
    => options.UseSqlServer(connection));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authorize}/{action=Index}/{id?}");

app.Run();
