﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantage.Data.Models
{
    public class Infraction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InfractionID { get; set; }

        public string Name { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}