using crud.api.core.enums;
using crud.api.core.interfaces;
using jwt.simplify.entities;
using jwt.simplify.services;
using JWT.Simplify.exceptions;
using JWT.Simplify.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using strings.utils.extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Simplify.controllers
{
    public class AutenticateController : Controller
    {
        private readonly UserService _service;
        private static string Secret = "e2855505-79aa-438d-985c-2b6711923412";

        public AutenticateController(UserService service, TokenConfig tokenConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [AllowAnonymous]
        public async Task<ActionResult<List<IHandleMessage>>> Login(LoginModel model)
        {
            var task = Task.Run(() => {
                var result = new List<IHandleMessage>();

                if (string.IsNullOrWhiteSpace(model.User))
                {
                    result.Add(new HandleMessage(nameof(AutenticateException), "Type your user name or e-mail.", HandlesCode.InvalidField));
                }

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    result.Add(new HandleMessage(nameof(AutenticateException), "Type your password.", HandlesCode.InvalidField));
                }

                if (result.Any())
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, result);
                }

                var hashValue = Criptography.GetHash(new string[] { model.User, model.Password });
                var user = this._service.GetData(u => u.HashIdEmail.Equals(hashValue) || u.HashIdLogin.Equals(hashValue)).FirstOrDefault();

                if (user is null)
                {
                    result.Add(new HandleMessage(nameof(AutenticateException), "Password or user is inválid.", HandlesCode.InvalidField));
                    return StatusCode((int)HttpStatusCode.BadRequest, result);
                }

                var token = GenerateToken(user);
                result.Add(new HandleMessage("AutenticateToken", token, HandlesCode.Accepted));

                return StatusCode((int)HttpStatusCode.BadRequest, result);
            });

            return await task;
        }

        private static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            user.Roles.ForEach(role =>
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role.Roler.ToString()));
            });

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
