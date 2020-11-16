using crud.api.core;
using crud.api.core.enums;
using crud.api.core.fieldType;
using crud.api.core.interfaces;
using jwt.consumer;
using Jwt.Simplify.Core.Entities;
using Jwt.Simplify.Core.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.AspNetCore.Mvc
{
    public static class AuthorizationCheck
    {
        public static string GetSystemName(this HttpRequest request)
        {
            if (request.Headers.ContainsKey("SystemSource"))
            {
                return request.Headers["SystemSource"].ToString();
            }

            return string.Empty;
        }

        public static List<IHandleMessage> Authenticated(this HttpRequest request, string authUrl)
        {
            if (request.Headers.ContainsKey("Authorization"))
            {
                var token = request.Headers["Authorization"].ToString();

                return token.CheckToken(authUrl);
            }
            else
            {
                return new List<IHandleMessage>() { new HandleMessage("Unauthorized", "Access danied.", HandlesCode.InvalidField) };
            }
        }
    }
}
