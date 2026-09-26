using RSVPApp.DataAccess;
using RSVPApp.Models;

namespace RSVPApp
{
    public partial class MainPage : ContentPage
    {
        private readonly AuthenticationService authenticationService = new();
        public static bool IsGuest { get; set; }

        public static User? CurrentUser { get; set; }

        private readonly AppDatabase database = new();

        public MainPage()
        {
            InitializeComponent();
        }

        // Validates the hard-coded user credentials and opens the Events page.
        private async void OnLoginClicked(object? sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim() ?? "";
            string password = PasswordEntry.Text ?? "";

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlertAsync(
                    "Missing Information",
                    "Please enter your email and password.",
                    "OK");

                return;
            }

            bool authenticated =
                await authenticationService.AuthenticateUserAsync(
                    email,
                    password);

            User? user = null;

            if (authenticated)
            {
                user = await database.GetUserByEmailAsync(email);
            }

            if (authenticated && user is not null)
            {
                MainPage.IsGuest = false;
                MainPage.CurrentUser = user;

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
            MainPage.CurrentUser = null;
            await Navigation.PushAsync(new EventsPage());
        }

        // Opens the page used to create a new account.
        private async void OnCreateAccountClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUserPage());
        }
    }
}