using crud.api.core.enums;
using crud.api.core.interfaces;
using crud.api.core.repositories;
using crud.api.core.services;
using jwt.simplify.entities;
using JWT.Simplify.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace jwt.simplify.services
{
    public class UserService : BaseService<User>
    {
        public UserService(IRepository<User> repository) : base(repository)
        {
        }

        public IEnumerable<IHandleMessage> SaveData(User user, string password)
        {
            var emailVal = new string[] { user.Email, password };
            var loginVal = new string[] { user.Login, password };

            var hashIdEmail = emailVal.ToHash();
            var hashIdLogin = loginVal.ToHash();

            user.HashIdEmail = hashIdEmail;
            user.HashIdLogin = hashIdLogin;

            var result = new List<IHandleMessage>();
            var userData = this.GetData(e => e.Id.Equals(user.Id)).FirstOrDefault();

            /*Basic validations*/
            if (userData != null)
            {
                /*
                 * To new record the email and login can't repeat in database.
                 */
                if (this.GetData(e => e.Email.ToLower().Equals(user.Email.ToLower())).Any())
                {
                    result.Add(new HandleMessage(nameof(LoginFoundException), "There is this email or login.", HandlesCode.ManyRecordsFound));
                }

                if (this.GetData(e => e.Email.ToLower().Equals(user.Login.ToLower())).Any())
                {
                    result.Add(new HandleMessage(nameof(LoginFoundException), "There is this email or login.", HandlesCode.ManyRecordsFound));
                }
            }

            if (result.Any())
            {
                return result;
            }

            return base.SaveData(user);
        }
    }
}
