using crud.api.core.enums;
using crud.api.core.interfaces;
using crud.api.core.repositories;
using crud.api.core.services;
using Jwt.Simplify.Core.Entities;
using Jwt.Simplify.Core.Exceptions;
using JWT.Simplify.services;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IHandleMessage Login(string user, string password)
        {
            var userHash = (new string[] { user, password }).ToHash();
            var userData = this.GetData(e => e.HashIdEmail.Equals(userHash) || e.HashIdLogin.Equals(userHash)).FirstOrDefault();

            if (userData == null)
            {
                return new HandleMessage(nameof(LoginFoundException), "Invalid login or password.", HandlesCode.ValueNotFound);
            }

            var token = TokenService.GenerateToken(userData);

            return new HandleMessage("Token", token, HandlesCode.Accepted);
        }
    }
}
