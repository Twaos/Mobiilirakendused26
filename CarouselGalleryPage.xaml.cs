using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Shapes;
using Mobiilirakendused.Resources.Localization;

namespace Mobiilirakendused;

public partial class CarouselGalleryPage : ContentPage
{
    // ANDMEMUDEL
    public class CarouselItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
        public string Info { get; set; }
    }

    // KLASSI MUUTUJAD
    private CarouselView carouselView;
    private List<CarouselItem> items;
    private int position = 0;
    private IDispatcherTimer timer;
    private bool _isUserInteracting = false;

    public CarouselGalleryPage()
    {
        Title = AppResources.GalleryTitle;

        // ANDMED
        LaeAndmed();

        // CAROUSELVIEW
        carouselView = new CarouselView
        {
            ItemsSource = items,
            HeightRequest = 400,
            PeekAreaInsets = new Thickness(40, 0, 40, 0),
            ItemTemplate = new DataTemplate(() =>
            {
                var frame = new Frame
                {
                    CornerRadius = 20,
                    HasShadow = true,
                    Padding = 0,
                    Margin = new Thickness(10),
                    BackgroundColor = Colors.Black
                };

                var grid = new Grid();

                // Pilt
                var image = new Image { Aspect = Aspect.AspectFit, BackgroundColor = Colors.White };
                image.SetBinding(Image.SourceProperty, "ImageSource");

                // Gradient
                var gradient = new BoxView
                {
                    Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 1),
                        EndPoint = new Point(0, 0),
                        GradientStops = new GradientStopCollection
                        {
                            new GradientStop(Colors.Black.WithAlpha(0.8f), 0),
                            new GradientStop(Colors.Transparent, 1)
                        }
                    }
                };

                // Tekstid
                var titleLabel = new Label
                {
                    TextColor = Colors.White,
                    FontSize = 24,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(15, 0, 15, 0)
                };
                titleLabel.SetBinding(Label.TextProperty, "Title");

                var descLabel = new Label
                {
                    TextColor = Colors.LightGray,
                    FontSize = 14,
                    Margin = new Thickness(15, 5, 15, 15)
                };
                descLabel.SetBinding(Label.TextProperty, "Description");

                var textLayout = new VerticalStackLayout
                {
                    VerticalOptions = LayoutOptions.End,
                    Children = { titleLabel, descLabel }
                };

                grid.Children.Add(image);
                grid.Children.Add(gradient);
                grid.Children.Add(textLayout);

                frame.Content = grid;

                // Klõpsamise sündmus
                var tap = new TapGestureRecognizer();
                tap.Tapped += async (s, e) =>
                {
                    var tappedItem = ((Frame)s).BindingContext as CarouselItem;
                    if (tappedItem != null)
                    {
                        await DisplayAlertAsync(tappedItem.Title, tappedItem.Info, "OK");
                    }
                };
                frame.GestureRecognizers.Add(tap);

                return frame;
            })
        };

        // KUI KASUTAJA KERIB KÄSITSI, PEATAME TAIMERI
        carouselView.PositionChanged += (s, e) =>
        {
            if (_isUserInteracting)
            {
                ResetTimer();
            }
        };

        // INDICATORVIEW
        var indicatorView = new IndicatorView
        {
            IndicatorColor = Colors.Gray,
            SelectedIndicatorColor = Colors.DarkSlateBlue,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10)
        };
        carouselView.IndicatorView = indicatorView;


        Content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            Children = { carouselView, indicatorView }
        };


        AlustaTaimerit();
    }

    // ANDMETE LAADIMINE
    private void LaeAndmed()
    {
        items = new List<CarouselItem>
        {
            new CarouselItem
            {
                Title = "C#",
                Description = AppResources.CSharpDesc,
                ImageSource = "csharp_logo.png",
                Info = AppResources.CSharpInfo
            },
            new CarouselItem
            {
                Title = "Python",
                Description = AppResources.PythonDesc,
                ImageSource = "python_logo.png",
                Info = AppResources.PythonInfo
            },
            new CarouselItem
            {
                Title = "JavaScript",
                Description = AppResources.JsDesc,
                ImageSource = "javascript_logo.png",
                Info = AppResources.JsInfo
            },
            new CarouselItem
            {
                Title = "Java",
                Description = AppResources.JavaDesc,
                ImageSource = "java_logo.png",
                Info = AppResources.JavaInfo
            },
            new CarouselItem
            {
                Title = "C++",
                Description = AppResources.CppDesc,
                ImageSource = "cpp_logo.png",
                Info = AppResources.CppInfo
            }
        };
    }

    // TAIMERI KÄIVITAMINE
    private void AlustaTaimerit()
    {
        if (timer != null && timer.IsRunning) return;

        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(4);
        timer.Tick += (s, e) =>
        {
            if (items == null || items.Count == 0) return;
            position = (position + 1) % items.Count;

            if (!_isUserInteracting)
            {
                carouselView.Position = position;
            }
        };
        timer.Start();
    }

    
    private void ResetTimer()
    {
        timer?.Stop();
        position = carouselView.Position;
        _isUserInteracting = false;
        AlustaTaimerit();
    }

    // KUI LEHT MUUTUB NÄHTAVAKS (UUENDAB ANDMED)
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Uuendab pealkirja ja andmeid, et keelevahetus rakenduks
        Title = AppResources.GalleryTitle;
        LaeAndmed();
        carouselView.ItemsSource = items;
        carouselView.Position = 0;
        position = 0;
        _isUserInteracting = false;

        // Käivitab taimeri uuesti
        AlustaTaimerit();
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        timer?.Stop();
    }
}