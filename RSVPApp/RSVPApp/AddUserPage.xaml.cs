using System.Net.Mail;
using RSVPApp.DataAccess;
using RSVPApp.Models;

namespace RSVPApp;

public partial class AddUserPage : ContentPage
{
    private readonly AppDatabase database = new();
    private readonly AuthenticationService authenticationService = new();

    public AddUserPage()
    {
        InitializeComponent();
    }

    // Validates account information, saves the user locally,
    // and registers the user with the authentication web service.
    private async void OnOkClicked(object? sender, EventArgs e)
    {
        string name = NameEntry.Text?.Trim() ?? "";
        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";
        string phone = PhoneEntry.Text?.Trim() ?? "";

        // Make sure all required fields contain data.
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

        // Validate the email address format.
        try
        {
            MailAddress emailAddress = new(email);

            if (emailAddress.Address != email)
            {
                await DisplayAlertAsync(
                    "Invalid Email",
                    "Please enter a valid email address.",
                    "OK");

                return;
            }
        }
        catch
        {
            await DisplayAlertAsync(
                "Invalid Email",
                "Please enter a valid email address.",
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

        // Save the account to the local SQLite database.
        await database.AddUserAsync(newUser);

        // Register the same credentials with the authentication web service.
        bool registered =
            await authenticationService.RegisterUserAsync(newUser);

        if (!registered)
        {
            await DisplayAlertAsync(
                "Web Service Error",
                "The account was saved locally, but could not be registered with the authentication service.",
                "OK");

            return;
        }

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