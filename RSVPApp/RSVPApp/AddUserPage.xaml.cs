namespace RSVPApp;

public partial class AddUserPage : ContentPage
{
    public AddUserPage()
    {
        InitializeComponent();
    }

    // Validates that all account fields contain data.
    private async void OnOkClicked(object? sender, EventArgs e)
    {
        string name = NameEntry.Text?.Trim() ?? "";
        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";
        string phone = PhoneEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlertAsync(
                "Missing Information",
                "Please fill in all fields.",
                "OK");

            return;
        }

        await Navigation.PushAsync(new EventsPage());
    }

    // Cancels account creation and returns to the login page.
    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}