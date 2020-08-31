using crud.api.core.enums;
using crud.api.core.interfaces;
using crud.api.core.repositories;
using crud.api.core.services;
using jwt.simplify.entities;
using JWT.Simplify.exceptions;
using strings.utils.extensions;
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
            var hashIdEmail = Criptography.GetHash(new string[] { user.Email, password });
            var hashIdLogin = Criptography.GetHash(new string[] { user.Login, password });

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
                    result.Add(new HandleMessage(nameof(LoginFoundException), "There is this email.", HandlesCode.ManyRecordsFound));
                }

                if (this.GetData(e => e.Email.ToLower().Equals(user.Login.ToLower())).Any())
                {
                    result.Add(new HandleMessage(nameof(LoginFoundException), "There is this login.", HandlesCode.ManyRecordsFound));
                }
            }
            else 
            {
                /*
                 * User can't change value to login and email.
                 */
                if (!userData.Email.ToLower().Equals(user.Email.ToLower()))
                {
                    result.Add(new HandleMessage(nameof(ChangeUserException), "Can't change email.", HandlesCode.ManyRecordsFound));
                }

                if (!userData.Login.ToLower().Equals(user.Login.ToLower()))
                {
                    result.Add(new HandleMessage(nameof(ChangeUserException), "Can't change login.", HandlesCode.ManyRecordsFound));
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
