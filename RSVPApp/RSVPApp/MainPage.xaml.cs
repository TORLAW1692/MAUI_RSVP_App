namespace RSVPApp
{
    public partial class MainPage : ContentPage
    {
        public static bool IsGuest { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        // Validates the hard-coded user credentials and opens the Events page.
        private async void OnLoginClicked(object? sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim() ?? "";
            string password = PasswordEntry.Text ?? "";

            if (email == "tory@example.com" && password == "Password1")
            {
                MainPage.IsGuest = false;
                await Navigation.PushAsync(new EventsPage());
            }
            else
            {
                await DisplayAlertAsync(
                    "Login Failed",
                    "Invalid email or password.",
                    "OK");
            }
        }

        // Allows the user to enter the app without registered credentials.
        private async void OnGuestLoginClicked(object? sender, EventArgs e)
        {
            MainPage.IsGuest = true;
            await Navigation.PushAsync(new EventsPage());
        }

        // Opens the page used to create a new account.
        private async void OnCreateAccountClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUserPage());
        }
    }
}