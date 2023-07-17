using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TinyCRM.API.Migrations
{
    /// <inheritdoc />
    public partial class seeddataforAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ProductDeals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProductDeals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Address", "CreatedBy", "CreatedDate", "Email", "Name", "Phone", "TotalSale", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("010d0dd0-09c4-47cc-86e2-edcb23632e81"), "Address - 1", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4752), "account1@gmail.com", "Account 1", "1", 0m, null, null },
                    { new Guid("05424e00-595c-4920-b4c3-ac33e588fd12"), "Address - 6", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4803), "account6@gmail.com", "Account 6", "6", 0m, null, null },
                    { new Guid("0a034c7e-032a-4a20-b717-25c5414f6ffc"), "Address - 3", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4774), "account3@gmail.com", "Account 3", "3", 0m, null, null },
                    { new Guid("0b2db0fb-df41-4c47-971c-ca5b4b6e0eb7"), "Address - 5", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4797), "account5@gmail.com", "Account 5", "5", 0m, null, null },
                    { new Guid("3dac8fb8-9107-47b4-bb40-c8f202ffa729"), "Address - 2", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4768), "account2@gmail.com", "Account 2", "2", 0m, null, null },
                    { new Guid("45ef8fa4-a5ea-4c42-85a8-e1260a88c1b1"), "Address - 8", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4812), "account8@gmail.com", "Account 8", "8", 0m, null, null },
                    { new Guid("5ecc86cc-b757-4f43-8e91-894ddb477d96"), "Address - 9", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4842), "account9@gmail.com", "Account 9", "9", 0m, null, null },
                    { new Guid("c03445ba-0d11-4231-a790-cde72d446924"), "Address - 4", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4792), "account4@gmail.com", "Account 4", "4", 0m, null, null },
                    { new Guid("cfdfb404-274c-48cd-bb07-3157ce54057c"), "Address - 10", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4850), "account10@gmail.com", "Account 10", "10", 0m, null, null },
                    { new Guid("d904216b-8edb-4b76-83e6-a1cfc55d5148"), "Address - 7", null, new DateTime(2023, 7, 14, 14, 35, 0, 715, DateTimeKind.Local).AddTicks(4808), "account7@gmail.com", "Account 7", "7", 0m, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("010d0dd0-09c4-47cc-86e2-edcb23632e81"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("05424e00-595c-4920-b4c3-ac33e588fd12"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0a034c7e-032a-4a20-b717-25c5414f6ffc"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0b2db0fb-df41-4c47-971c-ca5b4b6e0eb7"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("3dac8fb8-9107-47b4-bb40-c8f202ffa729"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("45ef8fa4-a5ea-4c42-85a8-e1260a88c1b1"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("5ecc86cc-b757-4f43-8e91-894ddb477d96"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("c03445ba-0d11-4231-a790-cde72d446924"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("cfdfb404-274c-48cd-bb07-3157ce54057c"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("d904216b-8edb-4b76-83e6-a1cfc55d5148"));

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ProductDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProductDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
