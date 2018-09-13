using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TickMeHelpers
{
    public class UserManagement : ManagementBase<User>
    {
        public UserManagement(IConfiguration config):base(config, "user")
        {

        }
        
        public async Task<User> GetOrCreateUserByAuthId(User user)
        {
            var cosmos = await GetClient();
            IQueryable<User> query = cosmos.CreateDocumentQuery<User>(CollectionLink).Where(e => e.AuthId.ToString() == user.AuthId.ToString());
            var res = query.ToList().FirstOrDefault();
            if(res == null)
            {
                res.Id = Guid.NewGuid();
                await Upsert(user);
            }
            return res;
        }
    }
}
