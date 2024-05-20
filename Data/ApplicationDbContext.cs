using CRUD_Persons.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Persons.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person()
                {
                    ID = 135465,
                    Name = "Elsa",
                    LastName = "Pato",
                    Age = 20,
                    Occupation = "Tanke de BV",
                    Birthday = DateTime.Now

                },
                new Person()
                {
                    ID = 465465,
                    Name = "Elver",
                    LastName = "Galarga",
                    Age = 25,
                    Occupation = "Tanke de BV",
                    Birthday = DateTime.Now

                }
                );

        }
    }
}
 