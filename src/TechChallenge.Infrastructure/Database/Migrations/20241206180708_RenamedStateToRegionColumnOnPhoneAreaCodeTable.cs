using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechChallenge.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenamedStateToRegionColumnOnPhoneAreaCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "PhoneAreaCode",
                newName: "Region");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                table: "PhoneAreaCode",
                newName: "State");
        }
    }
}
