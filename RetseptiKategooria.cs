using System.Collections.Generic;

namespace Mobiilirakendused;

public class RetseptiKategooria : List<Retsept>
{
    public string Nimetus { get; set; }

    public RetseptiKategooria(string nimetus, IEnumerable<Retsept> retseptid) : base(retseptid)
    {
        Nimetus = nimetus;
    }
}