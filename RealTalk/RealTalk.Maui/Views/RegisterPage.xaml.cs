using RealTalk.Maui.ViewModels;

namespace RealTalk.Maui.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            BindingContext = new RegisterViewModel();
        }
    }
}
