using Microsoft.EntityFrameworkCore;
using Tree_Task_.Models;

namespace Tree_Task_.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Подключаемся к SQLite базе данных
        optionsBuilder.UseSqlite("Data Source=products.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка модели Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.Price)
                  .HasColumnType("decimal(18,2)");
            entity.Property(e => e.Category)
                  .HasMaxLength(50)
                  .HasDefaultValue("General");
        });
    }
}