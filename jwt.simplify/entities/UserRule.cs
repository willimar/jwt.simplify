using crud.api.core.entities;
using jwt.simplify.enums;

namespace jwt.simplify.entities
{
    public class UserRule : BaseEntity
    {
        public RulerType Roler { get; set; }
        public string RolerName { get; set; }
    }
}