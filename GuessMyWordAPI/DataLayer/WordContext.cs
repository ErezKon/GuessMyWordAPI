using GuessMyWordAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GuessMyWordAPI.DataLayer
{
    public class WordContext: DbContext
    {
        public DbSet<WordModel> Words { get; set; }
        public DbSet<WordMetadata> WordMetadata { get; set; }

        protected readonly IConfiguration Configuration;

        public WordContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to mysql with connection string from app settings
            var connectionString = Configuration.GetConnectionString("WebApiDatabase");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordModel>()
                .HasMany(w => w.Metadata)
                .WithOne()
                .HasPrincipalKey(w => w.ID);

            modelBuilder.Entity<WordMetadata>()
                .HasOne(m => m.Word)
                .WithMany(w => w.Metadata)
                .HasForeignKey(m => m.WordId)
                .HasPrincipalKey(m => m.ID);

            modelBuilder.Entity<WordModel>()
                .Navigation(w => w.Metadata)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
