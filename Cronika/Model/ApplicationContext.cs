using Cronika.Model.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cronika.Model {
	public class ApplicationContext : DbContext {
		public DbSet<Product> Products { get; set; }
		public DbSet<Year> Years { get; set; }
		public DbSet<Game> Games { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<TVSerial> TVSerials { get; set; }

		public ApplicationContext() {
			try {
				Database.EnsureCreated();

			}
			catch (Exception ex) {
				MessageBox.Show($"Database error, reinitializated. {ex.Message}");
				Database.EnsureDeleted();
				Database.EnsureCreated();
			}
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlite("data source = appdb.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new ProductConfiguration());
			modelBuilder.Entity<Year>().HasIndex(c => c.Number);
		}
	}
}
