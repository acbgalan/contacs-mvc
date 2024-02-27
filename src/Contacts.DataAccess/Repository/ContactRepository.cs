using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contacts.DataAccess.Data;
using Contacts.DataAccess.Repository.Contracts;
using Contacts.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.DataAccess.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationContext _context;

        public ContactRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contact entity)
        {
            await _context.Contacts.AddAsync(entity);
        }

        public async Task<Contact> GetAsync(int id)
        {
            return await _context.Contacts
                .Include(g => g.Groups)
                .Include(w => w.Websites)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Contact>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task UpdateAsync(Contact entity)
        {
            await Task.Run(() =>
            {
                _context.Contacts.Update(entity);
            });
        }

        public async Task DeleteAsync(int id)
        {
            Contact contact = await GetAsync(id);

            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }
        }

        public async Task DeleteAsync(Contact entity)
        {
            await Task.Run(() =>
            {
                _context.Contacts.Remove(entity);
            });
        }

        public async Task<bool> ExitsASync(int id)
        {
            return await _context.Contacts.AnyAsync(x => x.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IQueryable<Contact> GetAllIQueryable()
        {
            return _context.Contacts.AsQueryable();
        }
    }
}
