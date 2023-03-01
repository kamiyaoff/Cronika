using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronika.Model {
	public abstract class Product {
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; } = "Description...";
		public int Rating { get; set; }
		public string? ImageUri { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public int YearId { get; set; }
		public virtual Year Year { get; set; } = null!;
	}
}
