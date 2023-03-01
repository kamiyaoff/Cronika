using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cronika.Model {
	public class TVSerial : Product {
		public string? Genre { get; set; }
		public string? Type { get; set; }

		public TVSerial(string name, int rating) {
			Name = name;
			Rating = rating;
		}
	}
}
