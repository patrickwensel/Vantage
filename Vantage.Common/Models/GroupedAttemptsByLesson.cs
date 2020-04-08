using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Vantage.Common.Models
{
    [NotMapped]
    public class GroupedAttemptsByLesson
    {
        public Lesson Lesson { get; set; }

        public int HighScore { get; set; }

        public DateTime? DateCompleted { get; set; }

        public int TotalAttempts { get; set; }

        public int TotalTimes { get; set; }

        public bool IsComplete { get; set; }

        public List<Infraction> Infractions { get; set; }

        public List<GroupedInfractions> GroupedInfractions => GetGroupedInfractions();

        private List<GroupedInfractions> GetGroupedInfractions()
        {
            List<GroupedInfractions> groupedInfractions = new List<GroupedInfractions>();
            if (Infractions == null)
                return groupedInfractions;

            foreach(var groupedItem in Infractions.GroupBy(x => x.InfractionID))
            {
                groupedInfractions.Add(new GroupedInfractions()
                {
                    Infraction = groupedItem.First(),
                    Occurances = groupedItem.Count(),
                    Deduction = groupedItem.Sum(x => x.Deduction)
                });
            }

            return groupedInfractions;
        }
    }
}
