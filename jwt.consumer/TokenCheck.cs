using crud.api.core;
using crud.api.core.enums;
using crud.api.core.interfaces;
using jwt.consumer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace System
{
    public static class TokenCheck
    {
        public static List<IHandleMessage> CheckToken(this string token, string authUrl)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer", string.Empty).Trim());

                try
                {
                    var result = httpClient.GetAsync(authUrl).Result;

                    if (!result.IsSuccessStatusCode)
                    {
                        var error = new List<IHandleMessage>() {
                                new HandleMessage("Unauthorized", result.ReasonPhrase, (HandlesCode)result.StatusCode)
                            };
                        return error;
                    }

                    var content = result.Content.ReadAsStringAsync().Result;

                    var roles = JsonConvert.DeserializeObject<List<HandleMessageAbs>>(content);

                    return roles.Cast<IHandleMessage>().ToList();
                }
                catch (Exception e)
                {
                    return new List<IHandleMessage>() {
                            new HandleMessage("Unauthorized", "Access danied.", HandlesCode.InvalidField),
                            new HandleMessage(e)
                        };
                }
            }
        }
    }
}
