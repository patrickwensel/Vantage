using System.Collections.Generic;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IUserService
    {
        Task UpdateCredential(User user);

        Task<User> GetUserByUsername(string username);

        Task<IList<User>> GetUsers();
    }
}
