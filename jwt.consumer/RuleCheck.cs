using crud.api.core.enums;
using crud.api.core.fieldType;
using crud.api.core.interfaces;
using Jwt.Simplify.Core.Entities;
using Jwt.Simplify.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class RuleCheck
    {
        public static bool Authorized(this UserRule userRule, string rolerName, RulerType rulerType)
        {
            // invalid rule
            if (userRule.Status != RecordStatus.Active)
            {
                return false;
            }

            // Super users
            if (userRule.Roler.Equals(RulerType.AdminUser) || userRule.Roler.Equals(RulerType.SuperUser))
            {
                return true;
            }

            return userRule.RolerName.ToLower().Equals(rolerName.ToLower()) && userRule.Roler.Equals(rulerType);
        }

        public static bool Authorized(this List<UserRule> userRuleList, string rolerName, RulerType rulerType)
        {
            return userRuleList.Any(x => x.Authorized(rolerName, rulerType));
        }

        public static List<UserRule> ToRuleList(this IEnumerable<IHandleMessage> authList)
        {
            var result = new List<UserRule>();

            foreach (var item in authList)
            {
                result.Add(item.ToUserRule());
            }

            return result;
        }

        private static UserRule ToUserRule(this IHandleMessage handleMessage)
        {
            return JsonConvert.DeserializeObject<UserRule>(handleMessage.Message);
        }

        public static bool HasAccess(this IEnumerable<IHandleMessage> handleMessage, RulerType rulerType, string ruleName)
        {
            if (handleMessage.Any(x => x.Code == HandlesCode.Accepted && x.MessageType.Equals("IsAuthenticated") && x.Message.Equals("True")))
            {
                var authorized = handleMessage.Where(x => x.MessageType.Equals(@"http://schemas.microsoft.com/ws/2008/06/identity/claims/role")).ToRuleList().Authorized(ruleName, rulerType);

                if (!authorized)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static Guid GetAccountId(this IEnumerable<IHandleMessage> handleMessage, string systemName)
        {
            var systemRule = handleMessage.Where(x => x.MessageType.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/system"));

            if (systemRule.Any())
            {
                var result = Guid.Empty;

                foreach (var item in systemRule)
                {
                    var system = JsonConvert.DeserializeObject<AuthorizedSystem>(item.Message);

                    if (system.SystemName.ToLower().Equals(systemName.ToLower()))
                    {
                        return system.AccountId;
                    }
                }

                throw new UnauthorizedAccessException();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
