using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.DataAccess.Repository.Contracts
{
    public interface IUnitOfWork
    {
        IContactRepository ContactRepository { get; }
        IGroupRepository GroupRepository { get; }

        int Save();
        Task<int> SaveAsync();
    }
}
