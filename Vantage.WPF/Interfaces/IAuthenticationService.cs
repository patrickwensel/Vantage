using System.Threading.Tasks;
using Vantage.WPF.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserReturnObject> AuthenticateUser(string username, string password);
    }
}
