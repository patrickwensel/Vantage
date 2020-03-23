using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantage.Common.Models
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Pin { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("Group")]
        public int GroupID { get; set; }
        public Group Group { get; set; }

        public virtual List<Attempt> Attempts { get; set; }
    }
}
