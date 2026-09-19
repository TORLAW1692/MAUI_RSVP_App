using RSVPApp.DataAccess;
using RSVPApp.Models;

namespace RSVPApp;

public partial class EventDetailsPage : ContentPage
{
    private readonly AppDatabase database = new();
    private readonly Event selectedEvent;

    public EventDetailsPage(Event selectedEvent)
    {
        InitializeComponent();

        this.selectedEvent = selectedEvent;

        // Makes the selected event available to XAML bindings.
        BindingContext = selectedEvent;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadEventDetailsAsync();
    }

    // Loads information that is not stored directly in the Event record.
    private async Task LoadEventDetailsAsync()
    {
        User? host =
            await database.GetUserByIdAsync(selectedEvent.HostUserID);

        HostLabel.Text =
            $"Host: {host?.Name ?? "Unknown"}";

        List<RSVP> rsvps =
            await database.GetRSVPsForEventAsync(selectedEvent.EventID);

        AttendeeCountLabel.Text =
            $"Current Attendees: {rsvps.Count}";

        // Prepopulate RSVP information from the logged-in account.
        if (MainPage.CurrentUser is not null)
        {
            RsvpNameEntry.Text = MainPage.CurrentUser.Name;
            RsvpEmailEntry.Text = MainPage.CurrentUser.EmailAddress;
            RsvpPhoneEntry.Text = MainPage.CurrentUser.MobilePhoneNumber;

            RsvpNameEntry.IsReadOnly = true;
            RsvpEmailEntry.IsReadOnly = true;
            RsvpPhoneEntry.IsReadOnly = true;
        }
        else
        {
            RsvpNameEntry.Text = string.Empty;
            RsvpEmailEntry.Text = string.Empty;
            RsvpPhoneEntry.Text = string.Empty;
        }
    }

    private async void OnRsvpClicked(object? sender, EventArgs e)
    {
        if (MainPage.CurrentUser is null)
        {
            await DisplayAlertAsync(
                "Login Required",
                "You must be logged in to RSVP for an event.",
                "OK");

            return;
        }

        // Do not allow an RSVP after the deadline.
        if (DateTime.Now > selectedEvent.RSVPDeadline)
        {
            await DisplayAlertAsync(
                "RSVP Closed",
                "The RSVP deadline for this event has passed.",
                "OK");

            return;
        }

        // Prevent the same user from submitting more than one RSVP.
        bool alreadyRSVPed =
            await database.HasUserRSVPedAsync(
                MainPage.CurrentUser.UserID,
                selectedEvent.EventID);

        if (alreadyRSVPed)
        {
            await DisplayAlertAsync(
                "Already RSVP'd",
                "You have already RSVP'd for this event.",
                "OK");

            return;
        }

        List<RSVP> currentRSVPs =
            await database.GetRSVPsForEventAsync(selectedEvent.EventID);

        // Do not exceed the event's maximum attendance.
        if (currentRSVPs.Count >= selectedEvent.MaximumAllowedAttendees)
        {
            await DisplayAlertAsync(
                "Event Full",
                "This event has reached its maximum number of attendees.",
                "OK");

            return;
        }

        RSVP newRSVP = new()
        {
            EventID = selectedEvent.EventID,
            UserID = MainPage.CurrentUser.UserID
        };

        await database.AddRSVPAsync(newRSVP);

        await DisplayAlertAsync(
            "RSVP Received",
            $"Thanks, {MainPage.CurrentUser.Name}! Your RSVP has been recorded.",
            "OK");

        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}