﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IDriverService
    {
        Task<IList<Driver>> GetAllDrivers();

        Task UpdateDriver(Driver driver);

        Task<Driver> AddNewDriver(Driver driver);

        Task<Driver> DeleteDriver(int driverId);
    }
}
