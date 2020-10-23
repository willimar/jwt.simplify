using crud.api.core.repositories;
using data.provider.core;
using Jwt.Simplify.Core.Entities;

namespace jwt.simplify.repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IDataProvider provider) : base(provider)
        {
        }
    }
}
