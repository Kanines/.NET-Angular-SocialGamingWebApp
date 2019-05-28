using System.Collections.Generic;
using System.Threading.Tasks;
using KaniWebApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KaniWebApp.API.Data
{
    public class WebAppRepository : IWebAppRepository
    {
        private readonly DataContext _context;

        public WebAppRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Image> GetImage(int id)
        {
            var image = await _context.Images.FirstOrDefaultAsync( i => i.Id == id);

            return image;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(i => i.Images).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(i => i.Images).ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}