using Microsoft.EntityFrameworkCore;
using PublicAdressBook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicAddressBook.DataAccess.Context
{
    public class PublicAddressBookContext : DbContext
    {
        public PublicAddressBookContext(DbContextOptions<PublicAddressBookContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>().HasData(new Contact
            { 
                ContactId = 1,
                Name = "Dino Lozina",
                DateOfBirth = new DateTime(1986, 1, 8),
                Address = "Krbavska 9",
                MobilePhone = "+385 910 787878",
                HomePhone = "+385 910 787878",
                WorkPhone = "+385 910 787878",
            });

            modelBuilder.Entity<Contact>().HasData(new Contact
            {
                ContactId = 2,
                Name = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 8),
                Address = "Wonderland",
                MobilePhone = "+385 910 787878",
                HomePhone = "+385 (021 555-555)",
                WorkPhone = "+385 910 787878",
            });

            modelBuilder.Entity<Contact>().HasData(new Contact
            {
                ContactId = 3,
                Name = "Bruce Wayne",
                DateOfBirth = new DateTime(1992, 1, 8),
                Address = "Gotham",
                MobilePhone = "+385 910 787878",
                HomePhone = "+385 (021 555-444)",
                WorkPhone = "+385 910 787878",
            });

            modelBuilder.Entity<Contact>().HasData(new Contact
            {
                ContactId = 4,
                Name = "Lara Croft",
                DateOfBirth = new DateTime(1992, 1, 8),
                Address = "Slivno",
                MobilePhone = "+385 910 787878",
                HomePhone = "+385 (021 555-444)",
                WorkPhone = "+385 910 787878",
            });
        }
    }
}
