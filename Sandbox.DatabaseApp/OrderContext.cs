using Microsoft.EntityFrameworkCore;
using Sandbox.DatabaseApp.Model;

namespace Sandbox.DatabaseApp
{
    internal sealed class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public string DbPath { get; }

        public OrderContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "sqlite.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
