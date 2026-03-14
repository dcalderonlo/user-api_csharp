using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_api_csharp.src.Data.Migrations
{
  /// <inheritdoc />
  public partial class AddProductsSuppliersCategories : Migration
  {
      /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "Categories",
        columns: table => new
        {
          Id = table.Column<int>(type: "INTEGER", nullable: false)
            .Annotation("Sqlite:Autoincrement", true),
          Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Categories", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: "Suppliers",
        columns: table => new
        {
          Id = table.Column<int>(type: "INTEGER", nullable: false)
            .Annotation("Sqlite:Autoincrement", true),
          Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
          Contact = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Suppliers", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: "Products",
        columns: table => new
        {
          Id = table.Column<int>(type: "INTEGER", nullable: false)
            .Annotation("Sqlite:Autoincrement", true),
          Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
          Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
          Stock = table.Column<int>(type: "INTEGER", nullable: false),
          SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
          CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Products", x => x.Id);
          table.ForeignKey(
            name: "FK_Products_Categories_CategoryId",
            column: x => x.CategoryId,
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
            name: "FK_Products_Suppliers_SupplierId",
            column: x => x.SupplierId,
            principalTable: "Suppliers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
        });

      migrationBuilder.CreateIndex(
        name: "IX_Products_CategoryId",
        table: "Products",
        column: "CategoryId");

      migrationBuilder.CreateIndex(
        name: "IX_Products_SupplierId",
        table: "Products",
        column: "SupplierId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        name: "Products");

      migrationBuilder.DropTable(
        name: "Categories");

      migrationBuilder.DropTable(
        name: "Suppliers");
    }
  }
}
