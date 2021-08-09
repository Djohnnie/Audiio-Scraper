using AudiioScraper.Entities;
using AudiioScraper.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace AudiioScraper.DataAccess
{
    public class AudiioScraperDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<AssetToDownload> AssetsToDownload { get; set; }

        public AudiioScraperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetValue<string>("DB_CONNECTION_STRING");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetToDownload>(a =>
            {
                a.ToTable("ASSETS_TO_DOWNLOAD");
                a.HasKey(x => x.Id).IsClustered(false);
                a.HasIndex(x => x.SysId).IsUnique().IsClustered();
                a.Property(x => x.SysId).ValueGeneratedOnAdd();
                a.Property(x => x.Kind).HasConversion(new EnumToStringConverter<AudiioKind>());
            });
        }
    }
}