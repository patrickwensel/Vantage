using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantage.Common.Models
{
    public class Lesson
    {
        [Key]
        public int LessonID { get; set; }

        public string PackType { get; set; }
        public string PackID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int LessonOrder { get; set; }

        public virtual List<Attempt> Attempts { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
