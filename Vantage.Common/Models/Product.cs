using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vantage.Common.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public virtual List<Group> Groups { get; set; }
    }
}
