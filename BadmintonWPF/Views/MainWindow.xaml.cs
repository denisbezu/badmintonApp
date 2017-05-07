using System.Windows;
using badmintonDataBase.DataAccess;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BadmintonContext context;
        public MainWindow()
        {
            InitializeComponent();
        }

    }
}
