using Infraestructure.Database;
using Infraestructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByCredentials(string user, string pass);
    }
    public class UserRepository : IUserRepository
    {
        protected Context _ctx;
        public UserRepository(Context ctx)
        {
            _ctx = ctx;
        }
        public async Task<User> GetUserByCredentials(string user, string pass)
        {
            return _ctx.User.Where(x => x.UserName == user && x.Password == pass).FirstOrDefault();
        }
    }
}
