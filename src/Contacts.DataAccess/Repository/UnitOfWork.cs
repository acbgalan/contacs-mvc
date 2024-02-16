using Contacts.DataAccess.Data;
using Contacts.DataAccess.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private IContactRepository _contactRepository;
        private IGroupRepository _groupRepository;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public IContactRepository ContactRepository
        {
            get
            {
                if (_contactRepository == null)
                {
                    _contactRepository = new ContactRepository(_context);
                }

                return _contactRepository;
            }
        }

        public IGroupRepository GroupRepository
        {
            get
            {
                if (_groupRepository == null)
                {
                    _groupRepository = new GroupRepository(_context);
                }

                return _groupRepository;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
