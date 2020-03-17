using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantage.Data.Models
{
    public class Infraction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InfractionID { get; set; }

        public string Name { get; set; }
        public string Enum { get; set; }
        public int Deduction { get; set; }

        [ForeignKey("Attempt")]
        public int AttemptID { get; set; }
        public Attempt Attempt { get; set; }

    }
}
