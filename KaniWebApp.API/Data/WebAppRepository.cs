using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaniWebApp.API.Helpers;
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

        public async Task<Image> GetMainImageForUser(int userId)
        {
            return await _context.Images.Where(i => i.UserId == userId).FirstOrDefaultAsync(i => i.IsMain);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(i => i.Images).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(i => i.Images).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Nickname.CaseInsensitiveContains(userParams.Nickname));

            // TO DO: add ranks to user model
            //users = users.Where(u => u.Rank == userParams.Rank);

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}