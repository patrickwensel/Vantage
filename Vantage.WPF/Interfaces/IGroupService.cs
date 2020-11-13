using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IGroupService
    {
        Task<IList<Group>> GetGroups();

        Task<Group> GetGroup(int Id);

        Task<Group> AddGroup(Group group);
    }
}
