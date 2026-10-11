using System;
using Microsoft.Maui.Layouts;

namespace Mobiilirakendused;

public partial class DateTimePage : ContentPage
{
    DatePicker datePicker;
    TimePicker timePicker;
    Label datetimeLabel;
    AbsoluteLayout al;

    public DateTimePage()
    {
        InitializeComponent();

        datePicker = new DatePicker
        {
            MinimumDate = DateTime.Now.AddDays(-15),
            MaximumDate = DateTime.Now.AddDays(15),
            Date = DateTime.Now,
            HorizontalOptions = LayoutOptions.Center,
            Format = "D"
        };

        datePicker.DateSelected += (sender, e) =>
        {
            datetimeLabel.Text = $"Valitud kuupäev: \n{datePicker.Date:D}";
        };

        timePicker = new TimePicker
        {
            Time = DateTime.Now.TimeOfDay,
            HorizontalOptions = LayoutOptions.Center,
            Format = "T"
        };

        timePicker.PropertyChanged += (sender, e) =>
        {
            datetimeLabel.Text = $"Valitud kellaaeg: \n{timePicker.Time:T}";
        };

        datetimeLabel = new Label
        {
            Text = "Vali kuupäev või aeg",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        al = new AbsoluteLayout
        {
            Children = { datePicker, timePicker, datetimeLabel }
        };

        // Paigutame elemendid proportsionaalselt
        AbsoluteLayout.SetLayoutBounds(datePicker, new Rect(0.2, 0.2, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(datePicker, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(timePicker, new Rect(0.2, 0.4, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(timePicker, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(datetimeLabel, new Rect(0.5, 0.7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(datetimeLabel, AbsoluteLayoutFlags.PositionProportional);

        Content = al;
    }
}