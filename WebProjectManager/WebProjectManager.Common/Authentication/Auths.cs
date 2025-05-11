using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectManager.Common.Authentication
{
    public class Auths
    {
        public static async Task<(string UserId, string PhoneNumber, long Expires)> GetInfoFromToken(string tokenString)
        {
            (string UserId, string PhoneNumber, long Expires) result = (string.Empty, string.Empty, 0);

            if (!string.IsNullOrEmpty(tokenString))
            {
                var jwtEncodedString = tokenString.Replace("Bearer ", "");
                var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
                result.UserId = token.Claims.First(c => c.Type == "id").Value;
                result.Expires = long.Parse(token.Claims.First(c => c.Type == "exp").Value);
            }

            return await Task.FromResult(result);
        }
    }
}
