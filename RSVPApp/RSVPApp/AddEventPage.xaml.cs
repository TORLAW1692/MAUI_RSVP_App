using System.Net;

namespace RSVPApp;

public partial class AddEventPage : ContentPage
{
    public AddEventPage()
    {
        InitializeComponent();
    }

    // Validates the event information and continues to Event Details.
    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        string host = HostEntry.Text?.Trim() ?? "";
        string eventName = EventNameEntry.Text?.Trim() ?? "";
        string address = AddressEntry.Text?.Trim() ?? "";
        string maxAttendeesText = MaxAttendeesEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(eventName) ||
            string.IsNullOrWhiteSpace(address) ||
            string.IsNullOrWhiteSpace(maxAttendeesText))
        {
            await DisplayAlertAsync(
                "Missing Information",
                "Please fill in all fields.",
                "OK");

            return;
        }

        if (!int.TryParse(maxAttendeesText, out int maxAttendees) ||
            maxAttendees <= 0)
        {
            await DisplayAlertAsync(
                "Invalid Value",
                "Maximum attendees must be a positive number.",
                "OK");

            return;
        }

        // Event information is not saved permanently yet.
        await Navigation.PushAsync(new EventDetailsPage());
    }

    // Cancels event creation and returns to Events.
    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}