using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseHold.Migrations
{
    /// <inheritdoc />
    public partial class household1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "deliveryMethods",
                columns: table => new
                {
                    delivery_method_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cost = table.Column<double>(type: "float", nullable: false),
                    delivery_days = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveryMethods", x => x.delivery_method_id);
                });

            migrationBuilder.CreateTable(
                name: "orderStatuses",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderStatuses", x => x.status_id);
                });

            migrationBuilder.CreateTable(
                name: "parentCategories",
                columns: table => new
                {
                    parent_category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parentCategories", x => x.parent_category_id);
                });

            migrationBuilder.CreateTable(
                name: "paymentMethods",
                columns: table => new
                {
                    payment_method_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentMethods", x => x.payment_method_id);
                });

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.status_id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    supplier_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    legal_address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contact_person = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.supplier_id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "userRoles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRoles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "manufacturers",
                columns: table => new
                {
                    manufacturer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    country_id = table.Column<int>(type: "int", nullable: false),
                    support_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    support_email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manufacturers", x => x.manufacturer_id);
                    table.ForeignKey(
                        name: "FK_manufacturers_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    parent_category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                    table.ForeignKey(
                        name: "FK_categories_parentCategories_parent_category_id",
                        column: x => x.parent_category_id,
                        principalTable: "parentCategories",
                        principalColumn: "parent_category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "supplyDeliveries",
                columns: table => new
                {
                    supply_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    invoice_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    delivery_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_amount = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplyDeliveries", x => x.supply_id);
                    table.ForeignKey(
                        name: "FK_supplyDeliveries_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "statuses",
                        principalColumn: "status_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_supplyDeliveries_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "supplier_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    last_name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    registration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    discount = table.Column<double>(type: "float", nullable: false),
                    last_login_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_userRoles_role_id",
                        column: x => x.role_id,
                        principalTable: "userRoles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    article = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    discount_percent = table.Column<double>(type: "float", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    manufacturer_id = table.Column<int>(type: "int", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    weight_kg = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_manufacturers_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "manufacturers",
                        principalColumn: "manufacturer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "supplier_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    delivery_method_id = table.Column<int>(type: "int", nullable: false),
                    payment_method_id = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completion_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_amount = table.Column<int>(type: "int", nullable: false),
                    delivery_address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    order_comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    discount_at_order = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_deliveryMethods_delivery_method_id",
                        column: x => x.delivery_method_id,
                        principalTable: "deliveryMethods",
                        principalColumn: "delivery_method_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_orderStatuses_status_id",
                        column: x => x.status_id,
                        principalTable: "orderStatuses",
                        principalColumn: "status_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_paymentMethods_payment_method_id",
                        column: x => x.payment_method_id,
                        principalTable: "paymentMethods",
                        principalColumn: "payment_method_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    comment_text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    moderation_status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_comments_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "priceHistories",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    old_price = table.Column<double>(type: "float", nullable: false),
                    new_price = table.Column<double>(type: "float", nullable: false),
                    change_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priceHistories", x => x.history_id);
                    table.ForeignKey(
                        name: "FK_priceHistories_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productImages",
                columns: table => new
                {
                    image_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    is_primary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productImages", x => x.image_id);
                    table.ForeignKey(
                        name: "FK_productImages_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productTags",
                columns: table => new
                {
                    productTag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productTags", x => x.productTag_id);
                    table.ForeignKey(
                        name: "FK_productTags_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productTags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "supplyItems",
                columns: table => new
                {
                    supplyItem_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    purchase_price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplyItems", x => x.supplyItem_id);
                    table.ForeignKey(
                        name: "FK_supplyItems_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_supplyItems_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "supplier_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orderItems",
                columns: table => new
                {
                    orderItem_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price_at_order = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderItems", x => x.orderItem_id);
                    table.ForeignKey(
                        name: "FK_orderItems_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderItems_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_category_id",
                table: "categories",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_product_id",
                table: "comments",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_manufacturers_country_id",
                table: "manufacturers",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_orderItems_order_id",
                table: "orderItems",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orderItems_product_id",
                table: "orderItems",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_delivery_method_id",
                table: "orders",
                column: "delivery_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_payment_method_id",
                table: "orders",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_status_id",
                table: "orders",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_priceHistories_product_id",
                table: "priceHistories",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_productImages_product_id",
                table: "productImages",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_manufacturer_id",
                table: "products",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_supplier_id",
                table: "products",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_productTags_product_id",
                table: "productTags",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_productTags_tag_id",
                table: "productTags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_supplyDeliveries_status_id",
                table: "supplyDeliveries",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_supplyDeliveries_supplier_id",
                table: "supplyDeliveries",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_supplyItems_product_id",
                table: "supplyItems",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_supplyItems_supplier_id",
                table: "supplyItems",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "orderItems");

            migrationBuilder.DropTable(
                name: "priceHistories");

            migrationBuilder.DropTable(
                name: "productImages");

            migrationBuilder.DropTable(
                name: "productTags");

            migrationBuilder.DropTable(
                name: "supplyDeliveries");

            migrationBuilder.DropTable(
                name: "supplyItems");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "statuses");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "deliveryMethods");

            migrationBuilder.DropTable(
                name: "orderStatuses");

            migrationBuilder.DropTable(
                name: "paymentMethods");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "manufacturers");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "userRoles");

            migrationBuilder.DropTable(
                name: "parentCategories");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
