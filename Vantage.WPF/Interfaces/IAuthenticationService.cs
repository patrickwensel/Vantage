using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserReturnObject> AuthenticateUser(string username, string password);

        Task UpdateCredential(User user);

        Task<User> GetUserByUsername(string username);
    }
}
