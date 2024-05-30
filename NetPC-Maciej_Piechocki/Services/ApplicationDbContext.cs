using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetPC_Maciej_Piechocki.Models;

namespace NetPC_Maciej_Piechocki.Services
{
    public class ApplicationDbContext : IdentityDbContext<Contact>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            // Add categories to Category
            var categories = new List<Category>
            {
                new Category {Id = 1, Name="służbowy"},
                new Category {Id = 2, Name="prywatny"},
                new Category {Id = 3, Name="inny"}
            };

			// Add subcategories to SubCategory
			var subcategories = new List<SubCategory>
            {
                new SubCategory {Id = 1, Name="szef"},
                new SubCategory {Id = 2, Name="klient"}
            };

            builder.Entity<IdentityRole>().HasData(admin);
            builder.Entity<Category>().HasData(categories);
            builder.Entity<SubCategory>().HasData(subcategories);
        }
    }
}
