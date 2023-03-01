using Cronika.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cronika.View {
	public partial class AddRecordDialog : Window {
		private readonly int[] ratingList = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		private readonly string[] gameGenres = { "Action", "RPG", "Survival", "Fighting", "Horror",
			"Shooter", "Arcade", "Adventure", "Slasher", "Racing", "Simulator", "Strategy"};

		private readonly string[] cinemaGenres = { "Action", "Adventure", "Comedy", "Horror", "Western",
			"Fantasy", "Sci-Fi", "Historical", "Drama", "Thriller", "Military", "Detective", "Mistery"};
		private readonly string[] bookGenres = { "Fantasy", "Romance", "Adventure",
			"Mystery", "Horror", "Thriller", "Sci-Fi", "Historical", "Paranormal",
			"Art", "Memoir", "Self-improvement", "Guide", "Humor"};

		public readonly string? selectedYear;

		public AddRecordDialog() {
			InitializeComponent();
		}
		public AddRecordDialog(string year) : this() {
			selectedYear = year;
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
        }

		private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			string? value = ((ComboBoxItem)typeComboBox.SelectedValue).Content.ToString();
			if (value == "Game")
				GameSelected();
			if (value == "Movie")
				MovieSelected();
			if (value == "TV-Serial")
				SerialSelected();
			if (value == "Book")
				BookSelected();
		}

		private void GameSelected() {
			authorTextBox.IsEnabled = false;
			cinemaTypeComboBox.IsEnabled = false;
			numberOfPagesTextBox.IsEnabled = false;
			gameLengthTextBox.IsEnabled = true;
			genreComboBox.ItemsSource = gameGenres;
			genreComboBox.SelectedIndex = 0;
		}

		private void MovieSelected() {
			authorTextBox.IsEnabled = false;
			cinemaTypeComboBox.IsEnabled = true;
			numberOfPagesTextBox.IsEnabled = false;
			gameLengthTextBox.IsEnabled = false;
			genreComboBox.ItemsSource = cinemaGenres;
			cinemaTypeComboBox.SelectedIndex = 0;
			genreComboBox.SelectedIndex = 0;
		}

		private void SerialSelected() {
			authorTextBox.IsEnabled = false;
			cinemaTypeComboBox.IsEnabled = true;
			numberOfPagesTextBox.IsEnabled = false;
			gameLengthTextBox.IsEnabled = false;
			genreComboBox.ItemsSource = cinemaGenres;
			cinemaTypeComboBox.SelectedIndex = 0;
			genreComboBox.SelectedIndex = 0;
		}

		private void BookSelected() {
			authorTextBox.IsEnabled = true;
			cinemaTypeComboBox.IsEnabled = false;
			numberOfPagesTextBox.IsEnabled = true;
			gameLengthTextBox.IsEnabled = false;
			genreComboBox.ItemsSource = bookGenres;
			genreComboBox.SelectedIndex = 0;
		}

		private void typeComboBox_Loaded(object sender, RoutedEventArgs e) {
			typeComboBox.SelectedIndex = 0;
			genreComboBox.SelectedIndex = 0;
			ratingComboBox.ItemsSource = ratingList;
			ratingComboBox.SelectedIndex = 0;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
			this.Close();
		}

		private void ImageButton_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog dialog = new OpenFileDialog {
				Filter = "Images|*.jpeg;*.jpg;*.png;",
				Multiselect = false,
				Title = "Select image"
			};
			if (dialog.ShowDialog() == true) {
				imageUri.Text = dialog.FileName;
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e) {
			if (nameTextBox.Text == "") {
				MessageBox.Show("New record must have at least name.", "Warning", 
					MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			using ApplicationContext db = new ApplicationContext();
			int rating = (int)ratingComboBox.SelectedItem;
			string productType = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString()!;
			if (productType == "Game") 
				AddGame(db, rating);
			if (productType == "Movie")
				AddMovie(db, rating);
			if (productType == "TV-Serial")
				AddSerial(db, rating);
			if (productType == "Book") 
				AddBook(db, rating);
			this.DialogResult = true;
			this.Close();
		}

		private void AddGame(ApplicationContext db, int rating) {
			Game ProductToAdd = new Game(nameTextBox.Text, rating);
			ProductToAdd.Year = db.Years.FirstOrDefault(y => y.Number == int.Parse(selectedYear!))!;
			if (imageUri.Text != "")
				ProductToAdd.ImageUri = imageUri.Text;
			if (gameLengthTextBox.Text != "")
				ProductToAdd.GameLength = int.Parse(gameLengthTextBox.Text);
			ProductToAdd.Genre = genreComboBox.SelectedItem.ToString();
			db.Games.Add(ProductToAdd);
			db.SaveChanges();
		}

		private void AddMovie(ApplicationContext db, int rating) {
			Movie ProductToAdd = new Movie(nameTextBox.Text, rating);
			ProductToAdd.Year = db.Years.FirstOrDefault(y => y.Number == int.Parse(selectedYear!))!;
			if (imageUri.Text != "")
				ProductToAdd.ImageUri = imageUri.Text;
			ProductToAdd.Type = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();
			ProductToAdd.Genre = genreComboBox.SelectedItem.ToString();
			db.Movies.Add(ProductToAdd);
			db.SaveChanges();
		}

		private void AddSerial(ApplicationContext db, int rating) {
			TVSerial ProductToAdd = new TVSerial(nameTextBox.Text, rating);
			ProductToAdd.Year = db.Years.FirstOrDefault(y => y.Number == int.Parse(selectedYear!))!;
			if (imageUri.Text != "")
				ProductToAdd.ImageUri = imageUri.Text;
			ProductToAdd.Type = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();
			ProductToAdd.Genre = genreComboBox.SelectedItem.ToString();
			db.TVSerials.Add(ProductToAdd);
			db.SaveChanges();
		}

		private void AddBook(ApplicationContext db, int rating) {
			Book ProductToAdd = new Book(nameTextBox.Text, rating);
			ProductToAdd.Year = db.Years.FirstOrDefault(y => y.Number == int.Parse(selectedYear!))!;
			if (imageUri.Text != "")
				ProductToAdd.ImageUri = imageUri.Text;
			if (numberOfPagesTextBox.Text != "")
				ProductToAdd.NumberOfPages = int.Parse(numberOfPagesTextBox.Text);
			ProductToAdd.Author = authorTextBox.Text;
			ProductToAdd.Genre = genreComboBox.SelectedItem.ToString();
			db.Books.Add(ProductToAdd);
			db.SaveChanges();
		}
	}
}
