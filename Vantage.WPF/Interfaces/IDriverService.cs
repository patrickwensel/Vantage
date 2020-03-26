using System.Collections.Generic;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IDriverService
    {
        Task<IList<Driver>> GetAllDrivers();

        Task<IList<Driver>> GetDriversByGroupID(int Id);
    }
}
