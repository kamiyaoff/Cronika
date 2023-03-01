using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronika.Model {
	public class Game : Product {
		public string? Genre { get; set; }
		public int? GameLength { get; set; }

		public Game(string name, int rating) {
			Name = name;
			Rating = rating;
		}
	}
}
