using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_api_csharp.src.Data.Migrations;
/// <inheritdoc />
public partial class AddJwtAuthFields : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.AddColumn<string>(
      name: "PasswordHash",
      table: "Users",
      type: "TEXT",
      maxLength: 64,
      nullable: false,
      defaultValue: "");

    migrationBuilder.AddColumn<DateTime>(
      name: "RefreshTokenExpiresAt",
      table: "Users",
      type: "TEXT",
      nullable: true);

    migrationBuilder.AddColumn<string>(
      name: "RefreshTokenHash",
      table: "Users",
      type: "TEXT",
      maxLength: 64,
      nullable: true);
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropColumn(
      name: "PasswordHash",
      table: "Users");

    migrationBuilder.DropColumn(
      name: "RefreshTokenExpiresAt",
      table: "Users");

    migrationBuilder.DropColumn(
      name: "RefreshTokenHash",
      table: "Users");
  }
}
