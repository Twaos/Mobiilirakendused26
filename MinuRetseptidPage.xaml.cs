using System.Collections.Generic;
using System.Linq;

namespace Mobiilirakendused;

public partial class MinuRetseptidPage : ContentPage
{
    public MinuRetseptidPage()
    {
        InitializeComponent();
    }

    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LaeRetseptid();
    }

    private void LaeRetseptid()
    {
        
        var kõikRetseptid = FailiHaldur.LoeRetseptid();

        
        var grupeeritud = kõikRetseptid
            .GroupBy(r => r.Kategooria)
            .Select(g => new RetseptiKategooria(g.Key, g.ToList()))
            .ToList();

        
        RetseptidList.ItemsSource = grupeeritud;
    }
}