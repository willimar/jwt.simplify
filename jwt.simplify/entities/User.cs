using crud.api.core.entities;
using crud.api.core.attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security;
using System.Text;

namespace jwt.simplify.entities
{
    public class User: BaseEntity
    {
        [IsRequiredField]
        public string UserName { get; set; }
        [IsRequiredField]
        public string Login { get; set; }
        [IsRequiredField]
        public string Email { get; set; }
        [IsRequiredField]
        public string HashIdLogin { get; set; }
        [IsRequiredField]
        public string HashIdEmail { get; set; }
        [IsRequiredField]
        public List<UserRule> Roles { get; set; }
        public List<AuthorizedSystem> AuthorizedSystems { get; set; }
    }
}
