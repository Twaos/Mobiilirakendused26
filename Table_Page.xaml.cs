using System;
using System.Collections.Generic;

namespace Mobiilirakendused;

public partial class Table_Page : ContentPage
{
    TableView tabelview;
    SwitchCell sc;
    ImageCell ic;
    TableSection fotosection;
    EntryCell telefoniCell;
    EntryCell emailCell;
    EntryCell sonumiCell;
    EntryCell nimiCell;

    // Tervituste nimekiri (vähemalt 5 sõnumit)
    private List<string> tervitused = new List<string>()
    {
        "Head jõule ja rõõmsat uut aastat! 🎄",
        "Palju õnne sünnipäevaks! 🎂",
        "Rõõmsaid pühi ja head uut aastat! 🎉",
        "Head sõbrapäeva! ❤️",
        "Palju õnne ja edu! 🌟",
        "Häid lihavõttepühi! 🐣"
    };

    private Random rnd = new Random();

    public Table_Page()
    {
        InitializeComponent();

        // SWITCH CELL (Näita/Peida foto)
        sc = new SwitchCell { Text = "Näita veel" };
        sc.OnChanged += Sc_OnChanged;

        // IMAGE CELL (Pilt)
        ic = new ImageCell
        {
            ImageSource = ImageSource.FromFile("bob.jpg"),
            Text = "Foto nimetus",
            Detail = "Foto kirjeldus"
        };

        // FOTO SEKTSIOON 
        fotosection = new TableSection();

        // ENTRY CELL-ID
        nimiCell = new EntryCell
        {
            Label = "Nimi:",
            Placeholder = "Sisesta oma sõbra nimi",
            Keyboard = Keyboard.Text
        };

        telefoniCell = new EntryCell
        {
            Label = "Telefon",
            Placeholder = "Sisesta tel. number",
            Keyboard = Keyboard.Telephone
        };

        emailCell = new EntryCell
        {
            Label = "Email",
            Placeholder = "Sisesta email",
            Keyboard = Keyboard.Email
        };

        sonumiCell = new EntryCell
        {
            Label = "Sõnum",
            Placeholder = "Kirjuta sõnum...",
            Keyboard = Keyboard.Text
        };

        // TABELVIEW
        tabelview = new TableView
        {
            Intent = TableIntent.Form,
            Root = new TableRoot
            {
                new TableSection("Põhiandmed:")
                {
                    nimiCell
                },
                new TableSection("Kontaktandmed:")
                {
                    telefoniCell,
                    emailCell,
                    sc
                },
                new TableSection("Sõnum:")
                {
                    sonumiCell
                },
                new TableSection("Lisavõimalused:")
                {
                    new ViewCell
                    {
                        View = LooNuppudeRida()
                    }
                },
                fotosection
            }
        };

        Content = tabelview;
    }

    // ABIMEETOD: Nuppude rida
    private View LooNuppudeRida()
    {
        var helistaBtn = new Button
        {
            Text = "HELISTA",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            CornerRadius = 10,
            FontSize = 12
        };
        helistaBtn.Clicked += Helista_Clicked;

        var smsBtn = new Button
        {
            Text = "SAADA SMS",
            BackgroundColor = Colors.LightBlue,
            TextColor = Colors.Black,
            CornerRadius = 10,
            FontSize = 12
        };
        smsBtn.Clicked += Saada_sms_Clicked;

        var emailBtn = new Button
        {
            Text = "SAADA EMAIL",
            BackgroundColor = Colors.LightYellow,
            TextColor = Colors.Black,
            CornerRadius = 10,
            FontSize = 12
        };
        emailBtn.Clicked += Saada_email_Clicked;

        var onnitlusBtn = new Button
        {
            Text = "ÕNNITLUSED",
            BackgroundColor = Colors.LightPink,
            TextColor = Colors.Black,
            CornerRadius = 10,
            FontSize = 12
        };
        onnitlusBtn.Clicked += OnnitlusBtn_Clicked;

        return new HorizontalStackLayout
        {
            Spacing = 5,
            HorizontalOptions = LayoutOptions.Center,
            Children = { helistaBtn, smsBtn, emailBtn, onnitlusBtn }
        };
    }

    // SWITCH CELL
    private void Sc_OnChanged(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            fotosection.Title = "Foto:";
            fotosection.Add(ic);
            sc.Text = "Peida";
        }
        else
        {
            fotosection.Title = "";
            fotosection.Remove(ic);
            sc.Text = "Näita veel";
        }
    }

    // HELISTA
    private async void Helista_Clicked(object sender, EventArgs e)
    {
        string telefon = telefoniCell.Text;
        if (string.IsNullOrWhiteSpace(telefon))
        {
            await DisplayAlertAsync("Viga", "Palun sisesta telefoninumber!", "OK");
            return;
        }

        try
        {
            PhoneDialer.Default.Open(telefon);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Viga", "Helistamine ebaõnnestus: " + ex.Message, "OK");
        }
    }

    // SAADA SMS
    private async void Saada_sms_Clicked(object sender, EventArgs e)
    {
        string telefon = telefoniCell.Text;
        string sonum = sonumiCell.Text;

        if (string.IsNullOrWhiteSpace(telefon))
        {
            await DisplayAlertAsync("Viga", "Palun sisesta telefoninumber!", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(sonum))
        {
            sonum = "Tere tulemast! Saadan sõnumi";
        }

        try
        {
            var sms = new SmsMessage(sonum, new[] { telefon });
            await Sms.Default.ComposeAsync(sms);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Viga", "SMS-i saatmine ebaõnnestus: " + ex.Message, "OK");
        }
    }

    // SAADA EMAIL
    private async void Saada_email_Clicked(object sender, EventArgs e)
    {
        string email = emailCell.Text;
        string sonum = sonumiCell.Text;

        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlertAsync("Viga", "Palun sisesta e-maili aadress!", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(sonum))
        {
            sonum = "Tere tulemast! Saadan e-maili";
        }

        try
        {
            var e_mail = new EmailMessage
            {
                Subject = "Tervitus",
                Body = sonum,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string> { email }
            };

            if (Email.Default.IsComposeSupported)
            {
                await Email.Default.ComposeAsync(e_mail);
            }
            else
            {
                await DisplayAlertAsync("Viga", "E-maili saatmine pole selles seadmes toetatud", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Viga", "E-maili saatmine ebaõnnestus: " + ex.Message, "OK");
        }
    }

    // ÕNNITLUSED (vali tervitus, mis läheb sõnumi välja)
    private async void OnnitlusBtn_Clicked(object sender, EventArgs e)
    {
        // Kuvame kõik tervitused valikuna
        string valik = await DisplayActionSheet("Vali õnnitlus", "Loobu", null, tervitused.ToArray());

        if (valik != null && valik != "Loobu")
        {
            // Kirjutame valitud tervituse sõnumi välja
            sonumiCell.Text = valik;

            await DisplayAlertAsync("Õnnitlus valitud", "Õnnitlus on lisatud sõnumi välja. Nüüd saad valida, kas saata SMS või e-mail.", "OK");
        }
    }
}