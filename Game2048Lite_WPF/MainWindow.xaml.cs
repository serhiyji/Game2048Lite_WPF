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

namespace Game2048Lite_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel = null;
        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new ViewModel();
            this.DataContext = viewModel;
            this.viewModel.SetGrid(ref this.grid);
            this.viewModel.Init();
        }
    }
}
