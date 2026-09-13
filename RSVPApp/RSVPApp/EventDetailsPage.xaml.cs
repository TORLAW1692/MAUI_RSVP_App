namespace RSVPApp;

public partial class EventDetailsPage : ContentPage
{
    public EventDetailsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Registered users receive prepopulated RSVP information.
        if (!MainPage.IsGuest)
        {
            RsvpNameEntry.Text = "Tory Lawson";
            RsvpEmailEntry.Text = "tory@example.com";
            RsvpPhoneEntry.Text = "555-123-4567";
        }
        else
        {
            // Guests must enter their own RSVP information.
            RsvpNameEntry.Text = string.Empty;
            RsvpEmailEntry.Text = string.Empty;
            RsvpPhoneEntry.Text = string.Empty;
        }
    }

    private async void OnRsvpClicked(object? sender, EventArgs e)
    {
        string name = RsvpNameEntry.Text?.Trim() ?? "";
        string email = RsvpEmailEntry.Text?.Trim() ?? "";
        string phone = RsvpPhoneEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlertAsync(
                "Missing Information",
                "Please enter your name, email, and phone number to RSVP.",
                "OK");

            return;
        }

        await DisplayAlertAsync(
            "RSVP Received",
            $"Thanks, {name}! Your RSVP has been recorded.",
            "OK");

        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}