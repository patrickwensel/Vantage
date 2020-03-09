using System.Threading.Tasks;
using Vantage.Data.Models;

namespace Vantage.API.Services
{
    public interface IUserService
    {
        public User AuthenticateAsync(string username, string password);
    }
}
