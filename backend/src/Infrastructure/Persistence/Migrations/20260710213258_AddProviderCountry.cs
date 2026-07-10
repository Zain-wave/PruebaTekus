using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaTekus.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Providers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Providers");
        }
    }
}
