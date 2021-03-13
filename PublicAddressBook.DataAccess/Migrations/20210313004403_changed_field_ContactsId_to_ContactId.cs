using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicAddressBook.DataAccess.Migrations
{
    public partial class changed_field_ContactsId_to_ContactId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactsId",
                table: "Contacts",
                newName: "ContactId");

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "ContactId", "Address", "DateOfBirth", "HomePhone", "MobilePhone", "Name", "WorkPhone" },
                values: new object[,]
                {
                    { 1, "Krbavska 9", new DateTime(1986, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "+385 910 787878", "+385 910 787878", "Dino Lozina", "+385 910 787878" },
                    { 2, "Wonderland", new DateTime(1990, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "+385 (021 555-555)", "+385 910 787878", "John Doe", "+385 910 787878" },
                    { 3, "Gotham", new DateTime(1992, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "+385 (021 555-444)", "+385 910 787878", "Bruce Wayne", "+385 910 787878" },
                    { 4, "Slivno", new DateTime(1992, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "+385 (021 555-444)", "+385 910 787878", "Lara Croft", "+385 910 787878" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "ContactId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "ContactId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "ContactId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "ContactId",
                keyValue: 4);

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "Contacts",
                newName: "ContactsId");
        }
    }
}
