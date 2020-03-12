using System;
using System.Collections.Generic;
using System.Text;

namespace Vantage.WPF
{
    public class AnonymousIdentity : CustomIdentity
    {
        public AnonymousIdentity()
            : base(string.Empty, new List<string> { })
        { }
    }
}
