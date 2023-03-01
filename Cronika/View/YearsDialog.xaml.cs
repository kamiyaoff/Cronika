using Cronika.Model;
using Cronika.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cronika.View
{
    public partial class YearsDialog : Window
    {
		public string? SelectedYear { get; set; }
        public YearsDialog()
        {
            InitializeComponent();
		}

		private void CloseWindowButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
		}

        void OnYearSelected(object sender, RoutedEventArgs e) {
            Button? button = sender as Button;
            string? year = button?.Content.ToString();
			SelectedYear = year;
			this.DialogResult = true;
			this.Close();
		}

		private void yearsList_Loaded(object sender, RoutedEventArgs e) {
			using ApplicationContext db = new ApplicationContext();
			var years = db.Years.Select(y => y.Number).ToList();
			foreach (var year in years) {
				Button btn = new Button();
				btn.Style = (Style)Application.Current.FindResource("SelectYearButton");
				btn.Width = 100;
				btn.Height = 40;
				btn.FontSize = 25;
				btn.Content = year.ToString();
				btn.Click += OnYearSelected;
				yearsList.Children.Add(btn);
			}
		}
	}
}
