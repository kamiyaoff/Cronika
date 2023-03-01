using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronika.Model {
	public class Year {
		public int Id { get; set; }
		public int Number { get; set; }
		public virtual List<Product>? Products { get; set; }
	}
}
