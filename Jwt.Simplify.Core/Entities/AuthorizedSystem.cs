using System;

namespace Jwt.Simplify.Core.Entities
{
    public class AuthorizedSystem
    {
        public string SystemName { get; set; }
        public Guid AccountId { get; set; }
    }
}