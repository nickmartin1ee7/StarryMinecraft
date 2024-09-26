namespace StarryMinecraft.MainApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _vm;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = _vm = vm;
        }
    }
}
