using Microsoft.EntityFrameworkCore.Migrations;

namespace Hubtel.Wallets.Api.Migrations
{
    public partial class RemovedLengthConstraintFromAccountNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Wallets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c64fc8ed-fc76-4881-a832-6e5fb5100c21",
                column: "ConcurrencyStamp",
                value: "abad7ee2-4e6c-4d43-88fb-62c96f3eed3a");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Wallets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c64fc8ed-fc76-4881-a832-6e5fb5100c21",
                column: "ConcurrencyStamp",
                value: "a67b548c-6106-4e15-a2f7-1c0f323e1288");
        }
    }
}
