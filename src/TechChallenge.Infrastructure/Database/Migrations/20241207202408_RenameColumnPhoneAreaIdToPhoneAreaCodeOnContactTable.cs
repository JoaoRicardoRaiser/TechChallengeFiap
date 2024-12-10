using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechChallenge.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnPhoneAreaIdToPhoneAreaCodeOnContactTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_PhoneArea_PhoneAreaId",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "PhoneAreaId",
                table: "Contact",
                newName: "PhoneAreaCode");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_PhoneAreaId",
                table: "Contact",
                newName: "IX_Contact_PhoneAreaCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_PhoneArea_PhoneAreaCode",
                table: "Contact",
                column: "PhoneAreaCode",
                principalTable: "PhoneArea",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_PhoneArea_PhoneAreaCode",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "PhoneAreaCode",
                table: "Contact",
                newName: "PhoneAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_PhoneAreaCode",
                table: "Contact",
                newName: "IX_Contact_PhoneAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_PhoneArea_PhoneAreaId",
                table: "Contact",
                column: "PhoneAreaId",
                principalTable: "PhoneArea",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
