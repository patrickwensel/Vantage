using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.API.Models
{
    public class LessonInfo
    {        
        public int LessonID { get; set; }

        public string PackType { get; set; }
        public string PackID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int LessonOrder { get; set; }

        public HighScoreAttempt Attempt { get; set; }
    }
}
