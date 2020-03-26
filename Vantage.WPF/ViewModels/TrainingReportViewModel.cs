using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vantage.Common.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class TrainingReportViewModel : BaseViewModel, IViewModel
    {
        private readonly IGroupService _groupService;
        private readonly IDriverService _driverService;
        private IList<Driver> _drivers;

        public IList<Driver> Drivers 
        {
            get { return _drivers; }
            set { SetProperty(ref _drivers, value); }
        }

        public TrainingReportViewModel(IGroupService groupService, IDriverService driverService)
        {
            _groupService = groupService;
            _driverService = driverService;
        }

        public async Task FetchDrivers()
        {
            //Drivers = new List<Driver>();
            //Driver driver = new Driver()
            //{
            //    DriverID = 1,
            //    GroupID = 1,
            //    FirstName = "James",
            //    LastName = "Smith",
            //    UserName = "JamesSmith",
            //    Group = new Group() 
            //    { 
            //        GroupID = 1,
            //        Name = "Test Group",
            //        ProductID = 1,
            //        Product = new Common.Models.Product()
            //        {
            //            ProductID = 1,
            //            Name = "Test Product",
            //            Version = "1.0.0",
            //        },
            //        Drivers = new List<Driver>(),
            //    },
            //    IsActive = true,
            //    Pin = "Test",
            //    Attempts = new List<Attempt>() 
            //    { 
            //        new Attempt()
            //        { 
            //            AttemptID = 1,
            //            DriverID = 1,
            //            LessonID = 1,
            //            Lesson = new Lesson()
            //            {
            //                LessonID = 1,
            //                Name = "Lesson1",
            //                IsActive = true,
            //                PackID = "101",
            //                PackType = "Lesson PackType"
            //            },
            //            DateCompleted = new DateTime(2020, 1, 20),
            //            Score = 87,
            //            TimeToComplete = 13,
            //            CumulativeLessonTime = 13,
            //            IsComplete = true,
            //            Infractions = new List<Infraction>()
            //            {
            //                new Infraction()
            //                { 
            //                    InfractionID =1,
            //                    AttemptID = 1,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =2,
            //                    AttemptID = 1,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =3,
            //                    AttemptID = 1,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                }
            //            }
            //        },
            //        new Attempt()
            //        {
            //            AttemptID = 2,
            //            DriverID = 1,
            //            LessonID = 2,
            //            Lesson = new Lesson()
            //            {
            //                LessonID = 2,
            //                Name = "Lesson2",
            //                IsActive = true,
            //                PackID = "102",
            //                PackType = "Lesson PackType2"
            //            },
            //            DateCompleted = new DateTime(2020, 1, 20),
            //            Score = 57,
            //            TimeToComplete = 13,
            //            CumulativeLessonTime = 13,
            //            IsComplete = true,
            //            Infractions = new List<Infraction>()
            //            {
            //                new Infraction()
            //                {
            //                    InfractionID =4,
            //                    AttemptID = 2,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =5,
            //                    AttemptID = 2,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =6,
            //                    AttemptID = 2,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                }
            //            }
            //        }
            //    }
            //};
            //Driver driver2 = new Driver()
            //{
            //    DriverID = 2,
            //    GroupID = 1,
            //    FirstName = "Patrick",
            //    LastName = "Wensel",
            //    UserName = "PatrickWensel",
            //    Group = new Group()
            //    {
            //        GroupID = 1,
            //        Name = "Test Group",
            //        ProductID = 1,
            //        Product = new Common.Models.Product()
            //        {
            //            ProductID = 1,
            //            Name = "Test Product",
            //            Version = "1.0.0",
            //        },
            //        Drivers = new List<Driver>(),
            //    },
            //    IsActive = true,
            //    Pin = "Test",
            //    Attempts = new List<Attempt>()
            //    {
            //        new Attempt()
            //        {
            //            AttemptID = 7,
            //            DriverID = 2,
            //            LessonID = 1,
            //            Lesson = new Lesson()
            //            {
            //                LessonID = 1,
            //                Name = "Lesson1",
            //                IsActive = true,
            //                PackID = "101",
            //                PackType = "Lesson PackType"
            //            },
            //            DateCompleted = new DateTime(2020, 1, 20),
            //            Score = 77,
            //            TimeToComplete = 13,
            //            CumulativeLessonTime = 13,
            //            IsComplete = true,
            //            Infractions = new List<Infraction>()
            //            {
            //                new Infraction()
            //                {
            //                    InfractionID =1,
            //                    AttemptID = 7,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =2,
            //                    AttemptID = 7,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =3,
            //                    AttemptID = 7,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                }
            //            }
            //        },
            //        new Attempt()
            //        {
            //            AttemptID = 8,
            //            DriverID = 1,
            //            LessonID = 2,
            //            Lesson = new Lesson()
            //            {
            //                LessonID = 2,
            //                Name = "Lesson2",
            //                IsActive = true,
            //                PackID = "102",
            //                PackType = "Lesson PackType2"
            //            },
            //            DateCompleted = new DateTime(2020, 1, 20),
            //            Score = 48,
            //            TimeToComplete = 13,
            //            CumulativeLessonTime = 13,
            //            IsComplete = true,
            //            Infractions = new List<Infraction>()
            //            {
            //                new Infraction()
            //                {
            //                    InfractionID =4,
            //                    AttemptID = 8,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =5,
            //                    AttemptID = 8,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                },
            //                new Infraction()
            //                {
            //                    InfractionID =6,
            //                    AttemptID = 8,
            //                    Enum = "Test Enum",
            //                    Name = "Didn't look both way",
            //                    Deduction = 5,
            //                }
            //            }
            //        }
            //    }
            //};

            //Drivers.Add(driver);
            //Drivers.Add(driver2);
            Drivers = await _driverService.GetAllDrivers();
            Console.WriteLine($"Drivers : {Drivers}");
        }
    }
}
