using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantage.Data.Models
{
    public class Attempt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttemptID { get; set; }

        public int Score { get; set; }
        public TimeSpan TimeToComplete { get; set; }
        public TimeSpan CumulativeLessonTime { get; set; }
        public DateTime DateCompleted { get; set; }
        public bool IsComplete { get; set; }

        [ForeignKey("Driver")]
        public int DriverID { get; set; }
        public Driver Driver { get; set; }

        [ForeignKey("Lesson")]
        public int LessonID { get; set; }
        public Lesson Lesson { get; set; }

        public virtual List<Infraction> Infractions { get; set; }
    }
}
