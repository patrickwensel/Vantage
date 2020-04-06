using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        [NotMapped]
        public List<GroupedAttemptsByLesson> GroupedAttemptsByLessons => GetGroupedAttempts();

        private List<GroupedAttemptsByLesson> GetGroupedAttempts()
        {
            var groupedAttempts = new List<GroupedAttemptsByLesson>();
            if (Attempts == null)
                return groupedAttempts;

            foreach (var groupedItems in Attempts.GroupBy(x => x.LessonID))
            {
                int highScore = groupedItems.Max(x => x.Score);
                Attempt highScoreAttempt = groupedItems.OrderByDescending(x => x.DateCompleted).First(x => x.Score == highScore);
                groupedAttempts.Add(new GroupedAttemptsByLesson()
                {
                    Lesson = groupedItems.First().Lesson,
                    TotalAttempts = groupedItems.Count(),
                    TotalTimes = groupedItems.Sum(x => x.TimeToComplete),
                    HighScore = highScore,
                    DateCompleted = highScoreAttempt.DateCompleted,
                    Infractions = highScoreAttempt.Infractions
                });
            }

            return groupedAttempts;
        }
    }
}
