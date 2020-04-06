using System;
using System.Collections.Generic;
using System.Text;

namespace Vantage.Common.Models
{
    public class GroupedInfractions
    {
        public Infraction Infraction { get; set; }

        public int Occurances { get; set; }
        
        public int Deduction { get; set; }
    }
}
