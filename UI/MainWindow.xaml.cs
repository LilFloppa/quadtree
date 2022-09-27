using QuadTree;
using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Domain domain;

        public MainWindow()
        {
            InitializeComponent();
            domain = new Domain(1600, 900);
            MainCanvas.SetDomain(domain);
        }
    }
}
