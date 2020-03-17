using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vantage.Data.Models
{
    public class Lesson
    {
        [Key]
        public int LessonID { get; set; }

        public string PackType { get; set; }
        public string PackID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual List<Attempt> Attempts { get; set; }
    }
}
