using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.API.Models
{
    public class DriverInfo
    {
        public int DriverID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Pin { get; set; }

        public bool IsActive { get; set; }

        public int? GroupID { get; set; }

        public Group Group { get; set; }

        public int ProductID { get; set; }

        public IList<LessonInfo> Lessons { get; set; }
    }
}
