using RSVPApp.DataAccess;
using RSVPApp.Models;

namespace RSVPApp;

public partial class AddUserPage : ContentPage
{
    private readonly AppDatabase database = new();

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

        // Prevent duplicate accounts using the same email address.
        User? existingUser = await database.GetUserByEmailAsync(email);

        if (existingUser is not null)
        {
            await DisplayAlertAsync(
                "Account Exists",
                "An account with this email address already exists.",
                "OK");

            return;
        }

        User newUser = new()
        {
            Name = name,
            EmailAddress = email,
            Password = password,
            MobilePhoneNumber = phone
        };

        await database.AddUserAsync(newUser);

        await DisplayAlertAsync(
            "Account Created",
            "Your account was created successfully.",
            "OK");

        await Navigation.PopAsync();
    }

    // Cancels account creation and returns to the login page.
    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}