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
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationContext _context;

        public GroupRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Group entity)
        {
            await _context.Groups.AddAsync(entity);
        }

        public async Task<Group> GetAsync(int id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task<List<Group>> GetAllAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task UpdateAsync(Group entity)
        {
            await Task.Run(() =>
            {
                _context.Groups.Update(entity);
            });
        }

        public async Task DeleteAsync(int id)
        {
            Group group = await GetAsync(id);

            if (group != null)
            {
                _context.Groups.Remove(group);
            }
        }

        public async Task DeleteAsync(Group entity)
        {
            await Task.Run(() =>
            {
                _context.Groups.Remove(entity);
            });
        }

        public async Task<bool> ExitsASync(int id)
        {
            return await _context.Groups.AnyAsync(x => x.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IQueryable<Group> GetAllIQueryableAsync()
        {
            return _context.Groups.AsQueryable();
        }
    }
}
