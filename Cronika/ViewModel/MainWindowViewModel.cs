using Cronika.Model;
using Cronika.View;
using Cronika.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Cronika.ViewModel {
	public class MainWindowViewModel : ObservableObject 
	{
		private UserControl? _currentContent;
		public UserControl? CurrentContent { 
			get => _currentContent;
			set {
				_currentContent = value;
				OnPropertyChanged();
			} 
		}

		private string? _selectedYear;
		public string? SelectedYear {
			get => _selectedYear;
			set {
				_selectedYear = value;
				OnPropertyChanged();
			}
		}

		private readonly Home homeView = new Home();
		private readonly ProductView productView = new ProductView();
		public RelayCommand HomeNavigateCommand { get; set; }
		public RelayCommand<object> ProductViewNavigateCommand { get; set; }
		public RelayCommand SelectYearCommand { get; set; }
		public RelayCommand AddYearCommand { get; set; }
		public RelayCommand DeleteYearCommand { get; set; }
		
		public MainWindowViewModel() {
			StartupConfigure();

			HomeNavigateCommand = new RelayCommand(() => CurrentContent = homeView);
			ProductViewNavigateCommand = new RelayCommand<object>(ProductViewCommandExecute);
			SelectYearCommand = new RelayCommand(SelectYearExecute, SelectYearCanExecute);
			AddYearCommand = new RelayCommand(AddYearExecute);
			DeleteYearCommand = new RelayCommand(DeleteYearExecute, DeleteYearCanExecute);
        }

		private void StartupConfigure() {
			using ApplicationContext db = new ApplicationContext();
			if (db.Years.Any()) {
				string? year = db.Years.ToList().Max(y => y.Number).ToString();
				SelectedYear = year;
				homeView.ViewModel.SelectedYear = year;
				productView.ViewModel.SelectedYear = year;
				homeView.ViewModel.UpdateHomeInfo();
			}
				CurrentContent = homeView;
				productView.ViewModel.ProductListChanged += RefreshList;
				productView.ViewModel.ProductListChanged += homeView.ViewModel.UpdateHomeInfo;
			
		}

		private void AddYearExecute() {
			using ApplicationContext db = new ApplicationContext();
			Year year = db.Years.Any() 
				? new Year { Number = db.Years.ToList().Last().Number + 1 }
				: new Year { Number = DateTime.Now.Year };
			string message = $"Are you sure you want to add new year: {year.Number}?";
			string title = "Confirm your action";
			var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes) {
				db.Years.Add(year);
				db.SaveChanges();
			}
		}

		private void DeleteYearExecute() {
			using ApplicationContext db = new ApplicationContext();
			var yearToDelete = db.Years.First(y => y.Number == int.Parse(SelectedYear!));
			string message = $"Are you sure you want to delete year: {yearToDelete.Number}?";
			string title = "Confirm your action";
			var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes) {
				db.Years.Remove(yearToDelete);
				db.SaveChanges();
				string? year = db.Years.ToList().LastOrDefault()?.Number.ToString();
				this.SelectedYear = year;
				productView.ViewModel.SelectedYear = year;
				homeView.ViewModel.SelectedYear = year;
				RefreshList();
				if (db.Years.Any())
					homeView.ViewModel.UpdateHomeInfo();
			}
		}

		private bool DeleteYearCanExecute() {
			using ApplicationContext db = new ApplicationContext();
			return SelectedYear is not null;
		}

		private void SelectYearExecute() {
			YearsDialog yearsDialog = new() {
				Owner = Application.Current.MainWindow
			};
			if (yearsDialog.ShowDialog() == true) {
				this.SelectedYear = yearsDialog.SelectedYear;
				productView.ViewModel.SelectedYear = yearsDialog.SelectedYear;
				homeView.ViewModel.SelectedYear = yearsDialog.SelectedYear;
				homeView.ViewModel.UpdateHomeInfo();
				RefreshList();
			}			
		}

		private bool SelectYearCanExecute() {
			using ApplicationContext db = new ApplicationContext();
			return db.Years.Any();
		}

		private void ProductViewCommandExecute(object parameter) {
			if (CurrentContent != productView)
				CurrentContent = productView;
			productView.ViewModel.SelectedCategory = parameter.ToString();
			RefreshList();
		}

		private void RefreshList() {
			productView.ProductListBox.ItemsSource = productView.ViewModel.SelectedCategory switch {
				"Games" => productView.ViewModel.Games,
				"Movies" => productView.ViewModel.Movies,
				"Anime/TV Serials" => productView.ViewModel.TVSerials,
				"Books" => productView.ViewModel.Books,
				_ => productView.ViewModel.Games,
			};
			productView.ProductListBox.SelectedIndex = 0;
		}

		/*private static void SeedDatabase() {
			using ApplicationContext db = new ApplicationContext();
			var year = new Year { Number = 2022 };
			var year2 = new Year { Number = 2023 };
			db.Years.AddRange(year, year2);
			db.Games.Add(new Game("Fallout New Vegas", 9) {
				Year = year,
				Genre = "RPG",
				GameLength = 66,
				Description = "Hueta",
			});
			db.Games.Add(new Game("Silent Hill 2", 5) {
				Year = year,
				Genre = "Horror",
				GameLength = 364,
				Description = "Ebat",
			});
			db.Games.Add(new Game("Naruto Shippuden: Ultimate Ninja Storm Revolution", 7) {
				Year = year,
				Genre = "Fighting",
				GameLength = 23,
				Description = "Zxc",
			});
			db.Games.Add(new Game("The Witcher 3", 10) {
				Year = year2,
				Genre = "RPG",
				GameLength = 124,
				Description = "Zxc",
			});
			db.Games.Add(new Game("Dark Souls 2", 8) {
				Year = year2,
				Genre = "RPG",
				GameLength = 43,
				Description = "Zxc",
			});
			db.Movies.Add(new Movie("Magagascar 3: Europe's Most Wanted", 8) {
				Year = year2,
				Genre = "Family",
				Description = "Zxc",
				Type = "Cartoon"
			});
			db.Movies.Add(new Movie("V/H/S 99", 6) {
				Year = year,
				Genre = "Horror",
				Description = "Zxc",
				Type = "Movie"
			});
			db.Books.Add(new Book("A Wild Sheep Chase", 10) {
				Author = "Haruki Murakami",
				Year = year,
				Genre = "Novel",
				Description = "Zxc",
				NumberOfPages = 450,
			});
			db.Books.Add(new Book("Crime and Punishment", 10) {
				Author = "Fyodor Dostoevsky",
				Year = year2,
				Genre = "Novel",
				Description = "Zxc",
				NumberOfPages = 800,
			});
			db.TVSerials.Add(new TVSerial("Tekken: Bloodline", 7) {
				Year = year2,
				Genre = "Action",
				Type = "Anime",
				Description = "Zxc",
			});
			db.TVSerials.Add(new TVSerial("The Boys", 8) {
				Year = year,
				Genre = "Horror",
				Type = "Anime",
				Description = "Zxc",
			});
			db.SaveChanges();
		}*/
	}
}
