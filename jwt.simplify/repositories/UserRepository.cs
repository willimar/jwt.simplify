using jwt.simplify.entities;
using System;
using System.Collections.Generic;
using System.Text;
using crud.api.core.repositories;
using data.provider.core;

namespace jwt.simplify.repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IDataProvider provider) : base(provider)
        {
        }
    }
}
