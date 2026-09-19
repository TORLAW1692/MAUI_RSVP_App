using System.Net;
using RSVPApp.DataAccess;
using RSVPApp.Models;

namespace RSVPApp;

public partial class AddEventPage : ContentPage
{
    private readonly AppDatabase database = new();
    public AddEventPage()
    {
        InitializeComponent();

        if (MainPage.CurrentUser is not null)
        {
            HostEntry.Text = MainPage.CurrentUser.Name;
            HostEntry.IsReadOnly = true;
        }
    }

    // Validates the event information and continues to Event Details.
    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (MainPage.CurrentUser is null)
        {
            await DisplayAlertAsync(
                "Login Required",
                "You must be logged in to create an event.",
                "OK");

            return;
        }

        string eventName = EventNameEntry.Text?.Trim() ?? "";
        string address = AddressEntry.Text?.Trim() ?? "";
        string maxAttendeesText = MaxAttendeesEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(eventName) ||
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

        DateTime eventDate = EventDatePicker.Date ?? DateTime.Today;
        TimeSpan eventTime = EventTimePicker.Time ?? TimeSpan.Zero;

        DateTime deadlineDate = RsvpDatePicker.Date ?? DateTime.Today;
        TimeSpan deadlineTime = RsvpTimePicker.Time ?? TimeSpan.Zero;

        DateTime eventDateTime = eventDate.Date + eventTime;
        DateTime rsvpDeadline = deadlineDate.Date + deadlineTime;

        RSVPApp.Models.Event newEvent = new()
        {
            HostUserID = MainPage.CurrentUser.UserID,
            EventName = eventName,
            EventAddress = address,
            MaximumAllowedAttendees = maxAttendees,
            EventDateTime = eventDateTime,
            RSVPDeadline = rsvpDeadline
        };

        await database.AddEventAsync(newEvent);

        await DisplayAlertAsync(
            "Event Created",
            "The event was saved successfully.",
            "OK");

        await Navigation.PopAsync();
    }

    // Cancels event creation and returns to Events.
    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}