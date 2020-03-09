using System.Threading.Tasks;
using Vantage.Data;
using Vantage.Data.Models;

namespace Vantage.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public User AuthenticateAsync(string username, string password)
        {
            //User user = new User();

            return null;

        }
    }
}
