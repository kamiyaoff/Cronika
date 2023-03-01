using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cronika.Model {
	public class Movie : Product {
		public string? Genre { get; set; }
		public string? Type { get; set; }

		public Movie(string name, int rating) {
			Name = name;
			Rating = rating;
		}
	}
}
