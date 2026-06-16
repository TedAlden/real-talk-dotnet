namespace RealTalk.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new LoginShell());

            CheckAuthStatus(window);

            return window;
        }

        private async void CheckAuthStatus(Window window)
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");

            if (!string.IsNullOrEmpty(token))
            {
                window.Page = new AppShell();
            }
        }
    }
}