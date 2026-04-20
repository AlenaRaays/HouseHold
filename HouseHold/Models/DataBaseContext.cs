namespace HouseHold.Models;
using Microsoft.EntityFrameworkCore;
public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options) { }

    public DbSet<Category> categories { get; set; }
    public DbSet<Comment> comments { get; set; }
    public DbSet<Country> countries { get; set; }
    public DbSet<DeliveryMethod> deliveryMethods{ get; set; }
    public DbSet<Manufacturer> manufacturers { get; set; }
    public DbSet<OrderItem> orderItems { get; set; }
    public DbSet<Orders> orders { get; set; }
    public DbSet<OrderStatus> orderStatuses { get; set; }
    public DbSet<ParentCategory> parentCategories { get; set; }
    public DbSet<PaymentMethod> paymentMethods { get; set; }
    public DbSet<PriceHistory> priceHistories { get; set; }
    public DbSet<Product> products { get; set; }
    public DbSet<ProductImage> productImages { get; set; }
    public DbSet<ProductTag> productTags { get; set; }
    public DbSet<Statuses> statuses { get; set; }
    public DbSet<Supplier> suppliers { get; set; }
    public DbSet<SupplyDelivery> supplyDeliveries { get; set; }
    public DbSet<SupplyItem> supplyItems { get; set; }
    public DbSet<Tag> tags { get; set; }
    public DbSet<UserRole> userRoles { get; set; }
    public DbSet<Users> users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Favorite> favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductTag>()
            .HasOne(x => x.Tag)
            .WithMany(y => y.productTags)
            .HasForeignKey(z => z.tag_id);
        modelBuilder.Entity<ProductTag>()
            .HasOne(x => x.Product)
            .WithMany(y => y.productTags)
            .HasForeignKey(z => z.product_id);
        modelBuilder.Entity<Category>()
            .HasOne(x => x.parentCategory)
            .WithMany(y => y.categories)
            .HasForeignKey(z => z.parent_category_id);
        modelBuilder.Entity<Product>()
            .HasOne(x => x.category)
            .WithMany(y => y.products)
            .HasForeignKey(z => z.category_id);
        modelBuilder.Entity<Product>()
            .HasOne(x => x.manufacturer)
            .WithMany(y => y.products)
            .HasForeignKey(z => z.manufacturer_id);
        modelBuilder.Entity<Product>()
            .HasOne(x => x.supplier)
            .WithMany(y => y.products)
            .HasForeignKey(z => z.supplier_id);
        modelBuilder.Entity<SupplyDelivery>()
            .HasOne(x => x.statuses)
            .WithMany(y => y.supplyDeliveries)
            .HasForeignKey(z => z.status_id);
        modelBuilder.Entity<SupplyDelivery>()
            .HasOne(x => x.supplier)
            .WithMany(y => y.supplyDeliveries)
            .HasForeignKey(z => z.supplier_id);
        modelBuilder.Entity<SupplyItem>()
            .HasOne(x => x.supplier)
            .WithMany(y => y.supplyItems)
            .HasForeignKey(z => z.supplier_id);
        modelBuilder.Entity<SupplyItem>()
            .HasOne(x => x.product)
            .WithMany(y => y.supplyItems)
            .HasForeignKey(z => z.product_id);
        modelBuilder.Entity<PriceHistory>()
            .HasOne(x => x.Product)
            .WithMany(y => y.priceHistories)
            .HasForeignKey(z => z.product_id);
        modelBuilder.Entity<ProductImage>()
            .HasOne(x => x.Product)
            .WithMany(y => y.productImages)
            .HasForeignKey(z => z.product_id);
        modelBuilder.Entity<Manufacturer>()
            .HasOne(x => x.country)
            .WithMany(y => y.manufacturers)
            .HasForeignKey(z => z.country_id);
        modelBuilder.Entity<Comment>()
            .HasOne(x => x.product)
            .WithMany(y => y.comments)
            .HasForeignKey(z => z.product_id);
        modelBuilder.Entity<Comment>()
            .HasOne(x => x.users)
            .WithMany(y => y.comments)
            .HasForeignKey(z => z.user_id);
        modelBuilder.Entity<OrderItem>()
            .HasOne(x => x.Product)
            .WithMany(y => y.orderItems)
            .HasForeignKey(z => z.product_id);
        modelBuilder.Entity<OrderItem>()
            .HasOne(x => x.Orders)
            .WithMany(y => y.orderItems)
            .HasForeignKey(z => z.order_id);
        modelBuilder.Entity<Orders>()
            .HasOne(x => x.deliveryMethod)
            .WithMany(y => y.orders)
            .HasForeignKey(z => z.delivery_method_id);
        modelBuilder.Entity<Orders>()
            .HasOne(x => x.paymentMethod)
            .WithMany(y => y.orders)
            .HasForeignKey(z => z.payment_method_id);
        modelBuilder.Entity<Orders>()
            .HasOne(x => x.status)
            .WithMany(y => y.orders)
            .HasForeignKey(z => z.status_id);
        modelBuilder.Entity<Orders>()
            .HasOne(x => x.user)
            .WithMany(y => y.orders)
            .HasForeignKey(z => z.user_id);
        modelBuilder.Entity<Users>()
            .HasOne(x => x.UserRole)
            .WithMany(y => y.Users)
            .HasForeignKey(z => z.role_id);
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.user_id)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.cart_id)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.product_id)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Favorite>()
        .HasIndex(f => new { f.user_id, f.product_id })
        .IsUnique();
        modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.user_id)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Product)
            .WithMany()
            .HasForeignKey(f => f.product_id)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
