using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginDemo.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoginDemo.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        // POST api/<controller>
        [Route("create")]
        [HttpPost]
        public IActionResult Post(User user)
        {
            TokenProvider _tokenProvider = new TokenProvider();
            var encodedJwt = _tokenProvider.LoginUser(user.USERID.Trim(), user.PASSWORD.Trim());
            var responseJson = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromDays(1).TotalSeconds
            };

            return Json(responseJson);
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
