using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cronika.Model {
	public class Book : Product {
		public string? Genre { get; set; }
		public string? Author { get; set; }
		public int? NumberOfPages { get; set; }

		public Book(string name, int rating) {
			Name = name;
			Rating = rating;
		}
	}
}
