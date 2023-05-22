using Microsoft.EntityFrameworkCore.Migrations;

namespace Hubtel.Wallets.Api.Migrations
{
    public partial class AddedPasswordHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c64fc8ed-fc76-4881-a832-6e5fb5100c21",
                column: "ConcurrencyStamp",
                value: "f52d353f-697a-4941-b268-b688c9cd6787");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "36861688-2b78-481e-946b-a1e057c1ed7e",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2cbfcda-f2d0-4489-93bb-89b664c4b7b9", "ADMIN@HUBTEL.COM", "AQAAAAEAACcQAAAAENhEbR+3L9l+4VOROJQzzvv+XBfnfpsnVLbhkeuN/ohkSWjGTbq4w/AXLRexeGzFAw==", "633a53e2-3635-45b7-abc9-ed6598a0186e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c64fc8ed-fc76-4881-a832-6e5fb5100c21",
                column: "ConcurrencyStamp",
                value: "9fb9f4dc-d97f-4296-a96f-5057991ed370");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "36861688-2b78-481e-946b-a1e057c1ed7e",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e79aab0-908b-4abd-8f5a-ccdccf2969d1", null, null, "48cd3605-7ae3-451a-b7be-3c369898ed90" });
        }
    }
}
