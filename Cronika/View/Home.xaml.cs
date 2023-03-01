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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cronika.View
{
    public partial class Home : UserControl
    {
        public HomeViewModel ViewModel { get; set; } = new HomeViewModel();
        public Home()
        {
            InitializeComponent();
        }
    }
}
