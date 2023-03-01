using Cronika.Model;
using Cronika.ViewModel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cronika.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public string? SelectedYear { get; set; }

        private Game? _bestGame;
		private Movie? _bestMovie;
		private TVSerial? _bestSerial;
		private Book? _bestBook;
		public Game? BestGame {
            get => _bestGame;
            set {
                _bestGame = value;
                OnPropertyChanged();
            }
        }
		public Movie? BestMovie {
			get => _bestMovie;
			set {
				_bestMovie = value;
				OnPropertyChanged();
			}
		}
		public TVSerial? BestSerial {
			get => _bestSerial;
			set {
				_bestSerial = value;
				OnPropertyChanged();
			}
		}
		public Book? BestBook {
			get => _bestBook;
			set {
				_bestBook = value;
				OnPropertyChanged();
			}
		}

		private int? _totalPlayTime;
		private int? _pagesTotalRead;
		private string? _favouriteGenre;
		private Game? _longestGame;
		public int? TotalPlayTime {
			get => _totalPlayTime;
			set {
				_totalPlayTime = value;
				OnPropertyChanged();
			}
		}
		public int? PagesTotalRead {
			get => _pagesTotalRead;
			set {
				_pagesTotalRead = value;
				OnPropertyChanged();
			}
		}
		public string? FavouriteGenre {
			get => _favouriteGenre;
			set {
				_favouriteGenre = value;
				OnPropertyChanged();
			}
		}
		public Game? LongestGame {
			get => _longestGame;
			set {
				_longestGame = value;
				OnPropertyChanged();
			}
		}

		private int? _gamesCount;
		private int? _moviesCount;
		private int? _serialsCount;
		private int? _booksCount;
		private int? _totalProducts;
		private Product? _lastProduct;

		public int? GamesCount {
			get => _gamesCount;
			set {
				_gamesCount = value;
				OnPropertyChanged();
			}
		}
		public int? MoviesCount {
			get => _moviesCount;
			set {
				_moviesCount = value;
				OnPropertyChanged();
			}
		}
		public int? SerialsCount {
			get => _serialsCount;
			set {
				_serialsCount = value;
				OnPropertyChanged();
			}
		}
		public int? BooksCount {
			get => _booksCount;
			set {
				_booksCount = value;
				OnPropertyChanged();
			}
		}
		public int? TotalProducts {
			get => _totalProducts;
			set {
				_totalProducts = value;
				OnPropertyChanged();
			}
		}
		public Product? LastProduct {
			get => _lastProduct;
			set {
				_lastProduct = value;
				OnPropertyChanged();
			}
		}

		private string? _lastProductType;
		public string? LastProductType {
			get => _lastProductType;
			set {
				_lastProductType = value;
				OnPropertyChanged();
			}
		}

		public HomeViewModel() { }

        public void UpdateHomeInfo() {
			using ApplicationContext db = new ApplicationContext();
			BestGame = db.Games.Where(g => g.Year.Number == int.Parse(SelectedYear!)).AsEnumerable().MaxBy(g => g.Rating);
			BestMovie = db.Movies.Where(g => g.Year.Number == int.Parse(SelectedYear!)).AsEnumerable().MaxBy(g => g.Rating);
			BestSerial = db.TVSerials.Where(g => g.Year.Number == int.Parse(SelectedYear!)).AsEnumerable().MaxBy(g => g.Rating);
			BestBook = db.Books.Where(g => g.Year.Number == int.Parse(SelectedYear!)).AsEnumerable().MaxBy(g => g.Rating);
			TotalPlayTime = db.Games.Where(g => g.Year.Number == int.Parse(SelectedYear!)).Sum(g => g.GameLength);
			PagesTotalRead = db.Books.Where(g => g.Year.Number == int.Parse(SelectedYear!)).Sum(b => b.NumberOfPages);
			try {
				FavouriteGenre = db.Games.Where(g => g.Year.Number == int.Parse(SelectedYear!))
					.GroupBy(g => g.Genre).AsEnumerable().OrderByDescending(g => g.Count()).First().First().Genre;
			}
			catch {
				FavouriteGenre = null;
			}
			LongestGame = db.Games.Where(g => g.Year.Number == int.Parse(SelectedYear!)).AsEnumerable().MaxBy(g => g.GameLength);
			GamesCount = db.Games.Where(g => g.Year.Number == int.Parse(SelectedYear!)).Count();
			MoviesCount = db.Movies.Where(g => g.Year.Number == int.Parse(SelectedYear!)).Count();
			SerialsCount = db.TVSerials.Where(g => g.Year.Number == int.Parse(SelectedYear!)).Count();
			BooksCount = db.Books.Where(g => g.Year.Number == int.Parse(SelectedYear!)).Count();
			TotalProducts = db.Products.Where(g => g.Year.Number == int.Parse(SelectedYear!)).Count();
			LastProduct = db.Products.Where(g => g.Year.Number == int.Parse(SelectedYear!)).OrderBy(c => c.CreatedAt).LastOrDefault();
			LastProductType = LastProduct?.GetType().ShortDisplayName();
		}
    }
}
