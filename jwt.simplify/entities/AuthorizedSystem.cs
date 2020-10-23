using System;

namespace jwt.simplify.entities
{
    public class AuthorizedSystem
    {
        public string SystemName { get; set; }
        public Guid AccountId { get; set; }
    }
}