using Microsoft.EntityFrameworkCore.Migrations;

namespace Hubtel.Wallets.Api.Migrations
{
    public partial class RemovedUserSeedingJobFromContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "36861688-2b78-481e-946b-a1e057c1ed7e", "c64fc8ed-fc76-4881-a832-6e5fb5100c21" });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "36861688-2b78-481e-946b-a1e057c1ed7e");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c64fc8ed-fc76-4881-a832-6e5fb5100c21",
                column: "ConcurrencyStamp",
                value: "a67b548c-6106-4e15-a2f7-1c0f323e1288");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c64fc8ed-fc76-4881-a832-6e5fb5100c21",
                column: "ConcurrencyStamp",
                value: "f52d353f-697a-4941-b268-b688c9cd6787");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "36861688-2b78-481e-946b-a1e057c1ed7e", 0, "a2cbfcda-f2d0-4489-93bb-89b664c4b7b9", "admin@hubtel.com", false, false, null, "ADMIN@HUBTEL.COM", "ADMIN@HUBTEL.COM", "AQAAAAEAACcQAAAAENhEbR+3L9l+4VOROJQzzvv+XBfnfpsnVLbhkeuN/ohkSWjGTbq4w/AXLRexeGzFAw==", "0202437997", false, "633a53e2-3635-45b7-abc9-ed6598a0186e", false, "admin@hubtel.com" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "36861688-2b78-481e-946b-a1e057c1ed7e", "c64fc8ed-fc76-4881-a832-6e5fb5100c21" });
        }
    }
}
