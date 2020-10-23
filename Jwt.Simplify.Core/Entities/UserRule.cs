using crud.api.core.entities;
using Jwt.Simplify.Core.Enums;

namespace Jwt.Simplify.Core.Entities
{
    public class UserRule : BaseEntity
    {
        public RulerType Roler { get; set; }
        public string RolerName { get; set; }
    }
}