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
	public partial class EditRecordDialog : Window {
		private readonly int[] ratingList = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		private readonly string[] gameGenres = { "Action", "RPG", "Survival", "Fighting", "Horror",
			"Shooter", "Arcade", "Adventure", "Slasher", "Racing", "Simulator", "Strategy"};

		private readonly string[] cinemaGenres = { "Action", "Adventure", "Comedy", "Horror", "Western",
			"Fantasy", "Sci-Fi", "Historical", "Drama", "Thriller", "Military", "Detective", "Mistery"};
		private readonly string[] bookGenres = { "Fantasy", "Romance", "Adventure",
			"Mystery", "Horror", "Thriller", "Sci-Fi", "Historical", "Paranormal",
			"Art", "Memoir", "Self-improvement", "Guide", "Humor"};

		public readonly string? selectedYear;

		private object? _product;

		public EditRecordDialog() {
			InitializeComponent();
		}

		public EditRecordDialog(object product) : this() {
			_product = product;
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void typeComboBox_Loaded(object sender, RoutedEventArgs e) {
			ratingComboBox.ItemsSource = ratingList;
			if (_product is Game game)
				LoadGame(game);
			if (_product is Movie movie) 
				LoadMovie(movie);
			if (_product is TVSerial serial) 
				LoadSerial(serial);
			if (_product is Book book) 
				LoadBook(book);
		}

		private void LoadGame(Game game) {
			typeComboBox.SelectedItem = "Game";
			typeComboBox.SelectedIndex = 0;
			authorTextBox.IsEnabled = false;
			cinemaTypeComboBox.IsEnabled = false;
			numberOfPagesTextBox.IsEnabled = false;
			gameLengthTextBox.IsEnabled = true;
			genreComboBox.ItemsSource = gameGenres;
			nameTextBox.Text = game.Name;
			ratingComboBox.SelectedIndex = game.Rating;
			imageUri.Text = game.ImageUri;
			gameLengthTextBox.Text = game.GameLength.ToString();
		}

		private void LoadMovie(Movie movie) {
			typeComboBox.SelectedValue = "Movie";
			typeComboBox.SelectedIndex = 1;
			authorTextBox.IsEnabled = false;
			cinemaTypeComboBox.IsEnabled = true;
			numberOfPagesTextBox.IsEnabled = false;
			gameLengthTextBox.IsEnabled = false;
			genreComboBox.ItemsSource = cinemaGenres;
			nameTextBox.Text = movie.Name;
			ratingComboBox.SelectedIndex = movie.Rating;
			imageUri.Text = movie.ImageUri;
		}

		private void LoadSerial(TVSerial serial) {
			typeComboBox.SelectedValue = "TV-Serial";
			typeComboBox.SelectedIndex = 2;
			authorTextBox.IsEnabled = false;
			cinemaTypeComboBox.IsEnabled = true;
			numberOfPagesTextBox.IsEnabled = false;
			gameLengthTextBox.IsEnabled = false;
			genreComboBox.ItemsSource = cinemaGenres;
			nameTextBox.Text = serial.Name;
			ratingComboBox.SelectedIndex = serial.Rating;
			imageUri.Text = serial.ImageUri;
		}

		private void LoadBook(Book book) {
			typeComboBox.SelectedValue = "Book";
			typeComboBox.SelectedIndex = 3;
			authorTextBox.IsEnabled = true;
			cinemaTypeComboBox.IsEnabled = false;
			numberOfPagesTextBox.IsEnabled = true;
			gameLengthTextBox.IsEnabled = false;
			genreComboBox.ItemsSource = bookGenres;
			nameTextBox.Text = book.Name;
			ratingComboBox.SelectedIndex = book.Rating;
			imageUri.Text = book.ImageUri;
			authorTextBox.Text = book.Author;
			numberOfPagesTextBox.Text = book.NumberOfPages.ToString();
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

		private void SaveButton_Click(object sender, RoutedEventArgs e) {
			using ApplicationContext db = new ApplicationContext();
			int rating = (int)ratingComboBox.SelectedItem;
			if (_product is Game gameProduct) 
				EditGame(db, gameProduct, rating);
			if (_product is Movie movieProduct) 
				EditMovie(db, movieProduct, rating);
			if (_product is TVSerial serialProduct) 
				EditSerial(db, serialProduct, rating);
			if (_product is Book bookProduct)
				EditBook(db, bookProduct, rating);

			this.DialogResult = true;
			this.Close();
		}

		private void EditGame(ApplicationContext db, Game gameProduct, int rating) {
			Game game = db.Games.FirstOrDefault(g => g.Id == gameProduct.Id)!;
			game.Name = nameTextBox.Text;
			game.Rating = rating;
			if (imageUri.Text != game.ImageUri)
				game.ImageUri = imageUri.Text;
			if (gameLengthTextBox.Text != "")
				game.GameLength = int.Parse(gameLengthTextBox.Text);
			game.Genre = genreComboBox.SelectedItem.ToString();
			db.SaveChanges();
		}

		private void EditMovie(ApplicationContext db, Movie movieProduct, int rating) {
			Movie movie = db.Movies.FirstOrDefault(m => m.Id == movieProduct.Id)!;
			movie.Name = nameTextBox.Text;
			movie.Rating = rating;
			if (imageUri.Text != movie.ImageUri)
				movie.ImageUri = imageUri.Text;
			movie.Type = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();
			movie.Genre = genreComboBox.SelectedItem.ToString();
			db.SaveChanges();
		}

		private void EditSerial(ApplicationContext db, TVSerial serialProduct, int rating) {
			TVSerial serial = db.TVSerials.FirstOrDefault(m => m.Id == serialProduct.Id)!;
			serial.Name = nameTextBox.Text;
			serial.Rating = rating;
			if (imageUri.Text != serial.ImageUri)
				serial.ImageUri = imageUri.Text;
			serial.Type = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();
			serial.Genre = genreComboBox.SelectedItem.ToString();
			db.SaveChanges();
		}

		private void EditBook(ApplicationContext db, Book bookProduct, int rating) {
			Book book = db.Books.FirstOrDefault(m => m.Id == bookProduct.Id)!;
			book.Name = nameTextBox.Text;
			book.Rating = rating;
			if (imageUri.Text != book.ImageUri)
				book.ImageUri = imageUri.Text;
			if (numberOfPagesTextBox.Text != "")
				book.NumberOfPages = int.Parse(numberOfPagesTextBox.Text);
			book.Author = authorTextBox.Text;
			book.Genre = genreComboBox.SelectedItem.ToString();
			db.SaveChanges();
		}
	}
}
