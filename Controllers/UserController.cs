using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Admins")] 
        [Authorize(Roles ="Administrator")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.GivenName} you are an {currentUser.Role}");
        }

        [HttpGet("Sellers")]
        [Authorize(Roles = "Seller")]
        public IActionResult SellersEndpoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.GivenName} you are an {currentUser.Role}");
        }

        [HttpGet("AdminsandSellers")]
        [Authorize(Roles = "Administrator,Seller")]
        public IActionResult AdminsandSellersEndpoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.GivenName} you are an {currentUser.Role}");
        }

        [HttpGet("Public")]
        public IActionResult Index() {
            return Ok("Hi you are on public property");
        }

        private UserModel GetCurrentUser() {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity !=null) {
                var userclaims = identity.Claims;
                return new UserModel
                {
                    Username = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    GivenName = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }
    }
}
