using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicAddressBook.DataAccess.Migrations
{
    public partial class added_separate_alternate_key_to_fields_name_and_address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Contacts_Name_Address",
                table: "Contacts");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Contacts_Address",
                table: "Contacts",
                column: "Address");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Contacts_Name",
                table: "Contacts",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Contacts_Address",
                table: "Contacts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Contacts_Name",
                table: "Contacts");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Contacts_Name_Address",
                table: "Contacts",
                columns: new[] { "Name", "Address" });
        }
    }
}
