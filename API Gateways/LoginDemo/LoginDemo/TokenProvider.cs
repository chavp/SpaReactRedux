using LoginDemo.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace LoginDemo
{
    public class TokenProvider
    {
        public string LoginUser(string UserID, string Password)
        {
            //Get user details for the user who is trying to login - JRozario
            var user = UserList.SingleOrDefault(x => x.USERID == UserID);

            //Authenticate User, Check if its a registered user in DB  - JRozario
            if (user == null)
                return null;

            //If its registered user, check user password stored in DB - JRozario
            //For demo, password is not hashed. Its just a string comparison - JRozario
            //In reality, password would be hashed and stored in DB. Before comparing, hash the password - JRozario
            if (Password == user.PASSWORD)
            {
                //Authentication successful, Issue Token with user credentials - JRozario
                //Provide the security key which was given in the JWToken configuration in Startup.cs - JRozario
                var key = Encoding.ASCII.GetBytes("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
                //Generate Token for user - JRozario
                var JWToken = new JwtSecurityToken(
                    issuer: "http://localhost:45092/",
                    audience: "http://localhost:45092/",
                    claims: GetUserClaims(user),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                    //Using HS256 Algorithm to encrypt Token - JRozario
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );

                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                return token;
            }
            else
            {
                return null;
            }
        }

        //Using hard coded collection list as Data Store for demo purpose. In reality, User data comes from Database or some other Data Source - JRozario
        private List<User> UserList = new List<User>
        {
            new User { USERID = "jsmith@email.com", PASSWORD = "test", EMAILID = "jsmith@email.com", FIRST_NAME = "John", LAST_NAME = "Smith", PHONE = "356-735-2748", ACCESS_LEVEL = "Director", READ_ONLY = "true" },
            new User { USERID = "srob@email.com", PASSWORD = "test", FIRST_NAME = "Steve", LAST_NAME = "Rob", EMAILID = "srob@email.com", PHONE = "567-479-8537", ACCESS_LEVEL = "Supervisor", READ_ONLY = "false" },
            new User { USERID = "dwill@email.com", PASSWORD = "test", FIRST_NAME = "DJ", LAST_NAME = "Will", EMAILID = "dwill@email.com", PHONE = "599-306-6010", ACCESS_LEVEL = "Analyst", READ_ONLY = "false" },
            new User { USERID = "JBlack@email.com", PASSWORD = "test", FIRST_NAME = "Joe", LAST_NAME = "Black", EMAILID = "JBlack@email.com", PHONE = "764-460-8610", ACCESS_LEVEL = "Analyst", READ_ONLY = "true" }
        };

        //Using hard coded collection list as Data Store for demo. In reality, User data comes from Database or some other Data Source - JRozario
        private IEnumerable<Claim> GetUserClaims(User user)
        {
            IEnumerable<Claim> claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.FIRST_NAME + " " + user.LAST_NAME),
                    new Claim("USERID", user.USERID),
                    new Claim("EMAILID", user.EMAILID),
                    new Claim("PHONE", user.PHONE),
                    new Claim("ACCESS_LEVEL", user.ACCESS_LEVEL.ToUpper()),
                    new Claim("READ_ONLY", user.READ_ONLY.ToUpper()),
                    };
            return claims;
        }
    }
}
