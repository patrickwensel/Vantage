using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vantage.API.Models
{
    public class HighScoreAttempt
    {
        public int AttemptID { get; set; }
        public int Score { get; set; }
        public int TimeToComplete { get; set; }
        public int CumulativeLessonTime { get; set; }
        public DateTime? DateCompleted { get; set; }
        public bool IsComplete { get; set; }
    }
}
