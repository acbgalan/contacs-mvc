using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contacts.Models;

namespace Contacts.DataAccess.Repository.Contracts
{
    public interface IGroupRepository : IRepositoryAsync<Group>
    {
        IQueryable<Group> GetAllIQueryable(); 
    }
}
