using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobiilirakendused;

public partial class RiikPage : ContentPage
{
    // Kasutame tavalist List'i (mitte ObservableCollection)
    private List<Riik> riigid;

    // UI elemendid
    private ListView list;
    private Entry entryNimi;
    private Entry entryPealinn;
    private Entry entryRahvaarv;
    private Entry entryLipp;
    private Button btnLisa;
    private Button btnKustuta;
    private Button btnSalvesta;
    private StackLayout põhiLayout;

    // Valitud riik
    private Riik valitudRiik = null;

    public RiikPage()
    {
        InitializeComponent();

        // ANDMED
        riigid = new List<Riik>
        {
            new Riik { Nimi = "Eesti", Pealinn = "Tallinn", Rahvaarv = 1330068, Lipp = "estonia.png" },
            new Riik { Nimi = "Soome", Pealinn = "Helsingi", Rahvaarv = 5540000, Lipp = "finland.png" },
            new Riik { Nimi = "Rootsi", Pealinn = "Stockholm", Rahvaarv = 10400000, Lipp = "sweden.png" },
            new Riik { Nimi = "Läti", Pealinn = "Riia", Rahvaarv = 1880000, Lipp = "latvia.png" }
        };

        // PEALKIRI
        var pealkiri = new Label
        {
            Text = "🇪🇺 Euroopa Riigid",
            FontSize = 26,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkBlue,
            Margin = new Thickness(0, 20, 0, 10)
        };

        // ENTRY LAHTRID
        entryNimi = new Entry { Placeholder = "Riigi nimi (nt Eesti)" };
        entryPealinn = new Entry { Placeholder = "Pealinn (nt Tallinn)" };
        entryRahvaarv = new Entry { Placeholder = "Rahvaarv (nt 1330068)", Keyboard = Keyboard.Numeric };
        entryLipp = new Entry { Placeholder = "Lipu failinimi (nt estonia.png)" };

        // NUPUD
        btnLisa = new Button { Text = "➕ Lisa", BackgroundColor = Colors.LightGreen, TextColor = Colors.Black, CornerRadius = 10 };
        btnLisa.Clicked += LisaNupp_Clicked;

        btnKustuta = new Button { Text = "🗑️ Kustuta valitud", BackgroundColor = Colors.LightCoral, TextColor = Colors.Black, CornerRadius = 10 };
        btnKustuta.Clicked += KustutaNupp_Clicked;

        btnSalvesta = new Button { Text = "💾 Salvesta muudatused", BackgroundColor = Colors.LightBlue, TextColor = Colors.Black, CornerRadius = 10 };
        btnSalvesta.Clicked += SalvestaNupp_Clicked;

        // LISTVIEW (algne loomine)
        list = LooListView();

        // KOGU PAIGUTUS
        põhiLayout = new StackLayout
        {
            Padding = 20,
            Spacing = 10,
            Children =
            {
                pealkiri,
                entryNimi,
                entryPealinn,
                entryRahvaarv,
                entryLipp,
                btnLisa,
                btnKustuta,
                btnSalvesta,
                list
            }
        };

        Content = põhiLayout;
    }

    // ListView loomine
    private ListView LooListView()
    {
        var lv = new ListView
        {
            HasUnevenRows = true,
            ItemsSource = riigid, // Värskendame andmeallikat
            ItemTemplate = new DataTemplate(() =>
            {
                Image imgLipp = new Image
                {
                    HeightRequest = 40,
                    WidthRequest = 60,
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center
                };
                imgLipp.SetBinding(Image.SourceProperty, "Lipp");

                Label lblNimi = new Label { FontSize = 18, FontAttributes = FontAttributes.Bold };
                lblNimi.SetBinding(Label.TextProperty, "Nimi");

                Label lblPealinn = new Label { FontSize = 14, TextColor = Colors.Gray };
                lblPealinn.SetBinding(Label.TextProperty, "Pealinn");

                var textLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center,
                    Children = { lblNimi, lblPealinn }
                };

                var rowLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(10),
                    Spacing = 15,
                    Children = { imgLipp, textLayout }
                };

                return new ViewCell { View = rowLayout };
            })
        };

        lv.ItemTapped += List_ItemTapped;
        return lv;
    }

    // LISTVIEW VÄRSKENDAMINE
    private void VärskendaListView()
    {
        // Eemaldame vana ListView
        põhiLayout.Children.Remove(list);

        // Loome uue ListView
        list = LooListView();

        // Lisame selle tagasi
        põhiLayout.Children.Add(list);

        // Tühjendame valiku
        valitudRiik = null;
        PuhastaLahtrid();
    }

    // LISTVIEW: ELEMENDI VALIMINE
    private async void List_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Riik selected = e.Item as Riik;
        if (selected == null) return;

        valitudRiik = selected;

        entryNimi.Text = selected.Nimi;
        entryPealinn.Text = selected.Pealinn;
        entryRahvaarv.Text = selected.Rahvaarv.ToString();
        entryLipp.Text = selected.Lipp;

        await DisplayAlertAsync("Riigi info",
            $"Riik: {selected.Nimi}\nPealinn: {selected.Pealinn}\nRahvaarv: {selected.Rahvaarv} inimest",
            "OK");
    }

    // LISA NUUPP
    private async void LisaNupp_Clicked(object sender, EventArgs e)
    {
        string uusNimi = entryNimi.Text?.Trim();

        if (string.IsNullOrWhiteSpace(uusNimi))
        {
            await DisplayAlertAsync("Viga", "Palun sisesta riigi nimi!", "OK");
            return;
        }

        bool riikOnOlemas = riigid.Any(r => r.Nimi.Equals(uusNimi, StringComparison.OrdinalIgnoreCase));

        if (riikOnOlemas)
        {
            await DisplayAlertAsync("Viga", "See riik on juba nimekirjas!", "OK");
            return;
        }

        int rahvaarv = 0;
        int.TryParse(entryRahvaarv.Text, out rahvaarv);

        string lipp = string.IsNullOrWhiteSpace(entryLipp.Text) ? "default_flag.png" : entryLipp.Text.Trim();

        riigid.Add(new Riik
        {
            Nimi = uusNimi,
            Pealinn = entryPealinn.Text?.Trim() ?? "",
            Rahvaarv = rahvaarv,
            Lipp = lipp
        });

        VärskendaListView();

        await DisplayAlertAsync("Lisatud", $"Riik '{uusNimi}' on nimekirja lisatud!", "OK");
    }

    // KUSTUTA NUUPP
    private async void KustutaNupp_Clicked(object sender, EventArgs e)
    {
        if (valitudRiik == null)
        {
            await DisplayAlertAsync("Viga", "Palun vali nimekirjast riik, mida kustutada!", "OK");
            return;
        }

        bool kinnitus = await DisplayAlertAsync("Kustuta", $"Kas soovid kustutada: {valitudRiik.Nimi}?", "Jah", "Ei");

        if (kinnitus)
        {
            string kustutatudNimi = valitudRiik.Nimi;

            // Eemaldame elemendi listist
            riigid.Remove(valitudRiik);

            VärskendaListView();

            await DisplayAlertAsync("Kustutatud", $"Riik '{kustutatudNimi}' on kustutatud!", "OK");
        }
    }

    // SALVESTA NUUPP
    private async void SalvestaNupp_Clicked(object sender, EventArgs e)
    {
        if (valitudRiik == null)
        {
            await DisplayAlertAsync("Viga", "Palun vali nimekirjast riik, mida muuta!", "OK");
            return;
        }

        string uusNimi = entryNimi.Text?.Trim();

        if (string.IsNullOrWhiteSpace(uusNimi))
        {
            await DisplayAlertAsync("Viga", "Riigi nimi ei tohi olla tühi!", "OK");
            return;
        }

        bool onDuplikaat = riigid.Any(r => r != valitudRiik && r.Nimi.Equals(uusNimi, StringComparison.OrdinalIgnoreCase));

        if (onDuplikaat)
        {
            await DisplayAlertAsync("Viga", "Selle nimega riik on juba olemas!", "OK");
            return;
        }

        // Uuendame andmeid otse objektis
        valitudRiik.Nimi = uusNimi;
        valitudRiik.Pealinn = entryPealinn.Text?.Trim() ?? "";
        int rahvaarv = 0;
        int.TryParse(entryRahvaarv.Text, out rahvaarv);
        valitudRiik.Rahvaarv = rahvaarv;
        valitudRiik.Lipp = string.IsNullOrWhiteSpace(entryLipp.Text) ? "default_flag.png" : entryLipp.Text.Trim();

        VärskendaListView();

        await DisplayAlertAsync("Salvestatud", "Muudatused on salvestatud!", "OK");
    }

    //ABIMEETOD: Puhasta lahtrid
    private void PuhastaLahtrid()
    {
        entryNimi.Text = "";
        entryPealinn.Text = "";
        entryRahvaarv.Text = "";
        entryLipp.Text = "";
    }
}