using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicAddressBook.DataAccess.Migrations
{
    public partial class added_alternate_key_to_fields_name_and_address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Contacts_Name_Address",
                table: "Contacts",
                columns: new[] { "Name", "Address" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Contacts_Name_Address",
                table: "Contacts");
        }
    }
}
