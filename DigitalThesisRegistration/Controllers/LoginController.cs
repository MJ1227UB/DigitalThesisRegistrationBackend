﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DigitalThesisRegistration.Helpers;
using DTRBLL.BusinessObjects;
using DTRBLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DigitalThesisRegistration.Controllers
{
    [Produces("application/json")]
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly IGroupService _groupService;

        private const string GroupPassword = "1234";
        private const string Supervisor = "Supervisor";
        private const string SupervisorPassword = "supervisorSecret";
        private const string Administrator = "Administrator";
        private const string AdminPassword = "adminSecret";

        public LoginController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        ///     Login
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Authorized token upon successful login</returns>
        [HttpPost]
        public IActionResult Login([FromBody] UserBO user)
        {
            switch (user.Username)
            {
                case Supervisor:
                    // Handle supervisor login
                    return HandleSupervisorLogin(user);
                case Administrator:
                    // Handle admin login
                    return HandleAdminLogin(user.Password);
                default:
                    var group = _groupService.Get(user.Username);
                    if (group == null) return Unauthorized();
                    return HandleGroupLogin(user);
            }
        }

        /// <summary>
        /// Verify group password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IActionResult HandleGroupLogin(UserBO user)
        {
            if (user.Password.Equals(GroupPassword))
                return Ok(GenerateToken(user));
            return Unauthorized();
        }

        /// <summary>
        /// Verify supervisor password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IActionResult HandleSupervisorLogin(UserBO user)
        {
            if (user.Password.Equals(SupervisorPassword))
                return Ok(GenerateToken(user));
            return Unauthorized();
        }

        /// <summary>
        /// Generate login token for authorized user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Token as string</returns>
        private string GenerateToken(UserBO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            switch (user.Username)
            {
                case "supervisor":
                    claims.Add(new Claim(ClaimTypes.Role, "Supervisor"));
                    break;
                case "admin":
                    claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                    break;
                default:
                    claims.Add(new Claim(ClaimTypes.Role, "Group"));
                    break;
            }

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    JwtSecurityKey.Key,
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(null, // issuer - not needed (ValidateIssuer = false)
                    null, // audience - not needed (ValidateAudience = false)
                    claims.ToArray(),
                    DateTime.Now, // notBefore
                    DateTime.Now.AddDays(1))); // expires

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// <summary>
        /// Verify admin password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private IActionResult HandleAdminLogin(string password)
        {
            if (password.Equals(AdminPassword))
            {
                //Handle authorized login
                return Ok();
            }
            return Unauthorized();
        }
    }
}