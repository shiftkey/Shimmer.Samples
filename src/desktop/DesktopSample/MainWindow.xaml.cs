namespace Shimmer.DesktopDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            AppBootstrapper = new AppBootstrapper();
            DataContext = AppBootstrapper;
        }

        public AppBootstrapper AppBootstrapper { get; set; }
    }
}
