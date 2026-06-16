using RealTalk.Maui.ViewModels;

namespace RealTalk.Maui.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }
    }
}
