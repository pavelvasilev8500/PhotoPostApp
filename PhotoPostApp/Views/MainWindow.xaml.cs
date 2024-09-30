using System.Windows;

namespace PhotoPostApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Drag_Move(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void minimaze_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
