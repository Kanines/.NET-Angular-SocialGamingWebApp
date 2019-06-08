using System.Collections.Generic;
using System.Threading.Tasks;
using KaniWebApp.API.Helpers;
using KaniWebApp.API.Models;

namespace KaniWebApp.API.Data
{
    public interface IWebAppRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams UserParams);
        Task<User> GetUser(int id);
        Task<Image> GetImage(int id);
        Task<Image> GetMainImageForUser(int userId);
        Task<Like> GetLike(int userId, int recipientId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}