namespace Mobiilirakendused;

public partial class Timer_Page : ContentPage
{
    public Timer_Page()
    {
        InitializeComponent();
    }

    bool on_off = true;

    private async void ShowTime()
    {
        while (on_off)
        {
            timer_btn.Text = DateTime.Now.ToString("T");
            await Task.Delay(1000);
        }
    }

    private void timer_btn_Clicked(object sender, EventArgs e)
    {
        if (on_off)
        {
            on_off = false;
        }
        else
        {
            on_off = true;
            ShowTime();
        }
    }

    private void tagasi_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        lbl.Text = "Tere!";
    }
}