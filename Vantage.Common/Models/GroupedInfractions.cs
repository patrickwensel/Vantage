using System.ComponentModel.DataAnnotations.Schema;

namespace Vantage.Common.Models
{
    [NotMapped]
    public class GroupedInfractions
    {
        public Infraction Infraction { get; set; }

        public int Occurances { get; set; }
        
        public int Deduction { get; set; }
    }
}
