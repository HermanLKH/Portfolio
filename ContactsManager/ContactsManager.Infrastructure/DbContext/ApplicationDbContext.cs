using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbContext
{
    // offer predefined dbset to store the users and roles
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set;}

        // to respect the configurations set in program.cs
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
        }

        // model builder -> configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // match c# models to database tables
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<Person>().ToTable("Person");

            // seed to countries
            // read from json file
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            // deserialize text into list of country objects
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson)!;

            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            // seed to persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson)!;

            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            // Fluent API
            // to configure a specific property of an entity
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("UNSTATED");

            // to configure entire table
            // set index to enable search & filter based on that property faster
            //modelBuilder.Entity<Person>()
            //    .HasIndex(temp => temp.TIN).IsUnique();

            // add check constraint
            modelBuilder.Entity<Person>()
               .HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"); // name of column (not refer to c# model)

            // table relations
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasOne<Country>(c => c.Country)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(p => p.CountryID);
            });
        }
        
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@Name", person.Name),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetter", person.ReceiveNewsLetter)
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @Name, @Email, @DateOfBirth," +
                "@Gender, @CountryID, @Address, @ReceiveNewsLetter", parameters);
		}
    }
}
