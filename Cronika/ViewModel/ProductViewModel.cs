using Cronika.Model;
using Cronika.View;
using Cronika.ViewModel.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Serialization;

namespace Cronika.ViewModel
{
	public class ProductViewModel : ObservableObject {
		private string? _selectedYear;
		public string? SelectedYear {
			get => _selectedYear;
			set {
				_selectedYear = value;
				OnPropertyChanged();
			}
		}

		private string? _selectedCategory;
		public string? SelectedCategory {
			get => _selectedCategory;
			set {
				_selectedCategory = value;
				OnPropertyChanged();
			}
		}

		public List<Game>? Games {
			get {
				using ApplicationContext db = new ApplicationContext();
				if (SelectedYear is null) return null;
				return db.Games.Include(g => g.Year).Where(g => g.Year.Number == int.Parse(SelectedYear)).ToList();
			}
		}
		public List<Movie>? Movies {
			get {
				using ApplicationContext db = new ApplicationContext();
				if (SelectedYear is null) return null;
				return db.Movies.Include(g => g.Year).Where(g => g.Year.Number == int.Parse(SelectedYear!)).ToList();
			}
		}
		public List<TVSerial>? TVSerials {
			get {
				using ApplicationContext db = new ApplicationContext();
				if (SelectedYear is null) return null;
				return db.TVSerials.Include(g => g.Year).Where(g => g.Year.Number == int.Parse(SelectedYear!)).ToList();
			}
		}
		public List<Book>? Books {
			get {
				using ApplicationContext db = new ApplicationContext();
				if (SelectedYear is null) return null;
				return db.Books.Include(g => g.Year).Where(g => g.Year.Number == int.Parse(SelectedYear!)).ToList();
			}
		}

		public event Action? ProductListChanged;

		public RelayCommand AddRecordCommand { get; set; }
		public RelayCommand<object> EditRecordCommand { get; set; }
		public RelayCommand<object> DeleteRecordCommand { get; set; }

		public ProductViewModel() {
			AddRecordCommand = new RelayCommand(AddRecordExecute, AddRecordCanExecute);
			EditRecordCommand = new RelayCommand<object>(EditRecordExecute, EditRecordCanExecute);
			DeleteRecordCommand = new RelayCommand<object>(DeleteRecordExecute, EditRecordCanExecute);
		}

		private void AddRecordExecute() {
			AddRecordDialog dialog = new AddRecordDialog(SelectedYear!);
			dialog.Owner = Application.Current.MainWindow;
			if (dialog.ShowDialog() == true) {
				ProductListChanged?.Invoke();
			}
		}

		private bool AddRecordCanExecute() => 
			SelectedYear is not null;

		private void EditRecordExecute(object product) {
			EditRecordDialog dialog = new EditRecordDialog(product);
			dialog.Owner = Application.Current.MainWindow;
			if (dialog.ShowDialog() == true)
				ProductListChanged?.Invoke();
		}

		private void DeleteRecordExecute(object product) {
			using ApplicationContext db = new ApplicationContext();
			if (product is Game game)
				db.Games.Remove(db.Games.First(g => g.Id == game.Id));
			if (product is Movie movie)
				db.Movies.Remove(db.Movies.First(g => g.Id == movie.Id));
			if (product is TVSerial serial)
				db.TVSerials.Remove(db.TVSerials.First(g => g.Id == serial.Id));
			if (product is Book book)
				db.Books.Remove(db.Books.First(g => g.Id == book.Id));
			db.SaveChanges();
			ProductListChanged?.Invoke();
		}

		private bool EditRecordCanExecute(object product) => 
			product is not null && product is Product;
	}
}
