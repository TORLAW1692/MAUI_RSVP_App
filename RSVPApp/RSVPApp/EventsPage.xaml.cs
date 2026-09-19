using System.Collections.ObjectModel;
using RSVPApp.DataAccess;
using RSVPApp.Models;

namespace RSVPApp;

public partial class EventsPage : ContentPage
{
    // Provides access to the SQLite database methods.
    private readonly AppDatabase database = new();

    // This collection is bound to the UI and controls which events are displayed.
    public ObservableCollection<Event> DisplayedEvents { get; set; } = new();

    public EventsPage()
    {
        InitializeComponent();

        // Allows the XAML page to access DisplayedEvents through data binding.
        BindingContext = this;
    }

    // Reloads events each time this page becomes visible.
    // This is useful after returning from the Add Event page.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadAllEventsAsync();
    }

    // Retrieves all events stored in SQLite.
    private async Task LoadAllEventsAsync()
    {
        List<Event> events = await database.GetEventsAsync();

        UpdateDisplayedEvents(events);
    }

    // Replaces the current displayed list with the supplied events.
    private void UpdateDisplayedEvents(IEnumerable<Event> events)
    {
        DisplayedEvents.Clear();

        foreach (Event eventItem in events)
        {
            DisplayedEvents.Add(eventItem);
        }
    }

    // Displays every event stored in the database.
    private async void OnAllEventsClicked(object? sender, EventArgs e)
    {
        await LoadAllEventsAsync();

        AllEventsButton.BackgroundColor = Color.FromArgb("#0A3B78");
        AttendingButton.BackgroundColor = Colors.Transparent;
        HostingButton.BackgroundColor = Colors.Transparent;
    }

    // Displays only events the logged-in user has RSVP'd to.
    private async void OnAttendingClicked(object? sender, EventArgs e)
    {
        if (MainPage.CurrentUser is null)
        {
            await DisplayAlertAsync(
                "Login Required",
                "You must be logged in to view events you are attending.",
                "OK");

            return;
        }

        // Retrieve RSVP records belonging to the current user.
        List<RSVP> rsvps =
            await database.GetRSVPsForUserAsync(MainPage.CurrentUser.UserID);

        // Retrieve all stored events so they can be filtered.
        List<Event> allEvents =
            await database.GetEventsAsync();

        // Store the event IDs that the current user has RSVP'd to.
        HashSet<int> attendingEventIds =
            rsvps.Select(r => r.EventID).ToHashSet();

        // Display only events whose IDs appear in the user's RSVP records.
        UpdateDisplayedEvents(
            allEvents.Where(e => attendingEventIds.Contains(e.EventID)));

        AllEventsButton.BackgroundColor = Colors.Transparent;
        AttendingButton.BackgroundColor = Color.FromArgb("#0A3B78");
        HostingButton.BackgroundColor = Colors.Transparent;
    }

    // Displays only events created by the logged-in user.
    private async void OnHostingClicked(object? sender, EventArgs e)
    {
        if (MainPage.CurrentUser is null)
        {
            await DisplayAlertAsync(
                "Login Required",
                "You must be logged in to view events you are hosting.",
                "OK");

            return;
        }

        List<Event> events =
            await database.GetHostedEventsAsync(MainPage.CurrentUser.UserID);

        UpdateDisplayedEvents(events);

        AllEventsButton.BackgroundColor = Colors.Transparent;
        AttendingButton.BackgroundColor = Colors.Transparent;
        HostingButton.BackgroundColor = Color.FromArgb("#0A3B78");
    }

    // Opens the Add Event page for logged-in users.
    private async void OnAddEventClicked(object? sender, EventArgs e)
    {
        if (MainPage.CurrentUser is null)
        {
            await DisplayAlertAsync(
                "Login Required",
                "You must be logged in to create an event.",
                "OK");

            return;
        }

        await Navigation.PushAsync(new AddEventPage());
    }

    // Logs the user out and returns to the login page.
    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        MainPage.CurrentUser = null;
        MainPage.IsGuest = false;

        await Navigation.PopToRootAsync();
    }

    // Opens the details page for the event the user tapped.
    // Opens the details page for the event that was tapped.
    private async void OnEventTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Event selectedEvent)
        {
            await Navigation.PushAsync(
                new EventDetailsPage(selectedEvent));
        }
    }
}