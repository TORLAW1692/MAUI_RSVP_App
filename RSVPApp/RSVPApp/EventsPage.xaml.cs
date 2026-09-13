namespace RSVPApp;

public partial class EventsPage : ContentPage
{
    public EventsPage()
    {
        InitializeComponent();
    }

    // Displays every available event.
    private void OnAllEventsClicked(object? sender, EventArgs e)
    {
        PotluckFrame.IsVisible = true;
        CharityFrame.IsVisible = true;
        GameNightFrame.IsVisible = true;

        AllEventsButton.BackgroundColor = Color.FromArgb("#0A3B78");
        AttendingButton.BackgroundColor = Colors.Transparent;
        HostingButton.BackgroundColor = Colors.Transparent;
    }
    // Displays only the events the user is attending.
    private void OnAttendingClicked(object? sender, EventArgs e)
    {
        PotluckFrame.IsVisible = true;
        CharityFrame.IsVisible = false;
        GameNightFrame.IsVisible = false;

        AllEventsButton.BackgroundColor = Colors.Transparent;
        AttendingButton.BackgroundColor = Color.FromArgb("#0A3B78");
        HostingButton.BackgroundColor = Colors.Transparent;
    }
    // Displays only the events the user is hosting.
    private void OnHostingClicked(object? sender, EventArgs e)
    {
        PotluckFrame.IsVisible = false;
        CharityFrame.IsVisible = false;
        GameNightFrame.IsVisible = true;

        AllEventsButton.BackgroundColor = Colors.Transparent;
        AttendingButton.BackgroundColor = Colors.Transparent;
        HostingButton.BackgroundColor = Color.FromArgb("#0A3B78");
    }
    // Opens the Add Event page.
    private async void OnAddEventClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddEventPage());
    }

    // Opens the selected event's details.
    private async void OnEventTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new EventDetailsPage());
    }

    // Returns the user to the login page.
    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}