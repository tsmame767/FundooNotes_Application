using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace RepositoryLayer.JWT
{
    public class ExtractToken
    {

        public string ExtractEmailFromToken(string token)
        {
            // Initialize a new instance of JwtSecurityTokenHandler to read the JWT.
            var handler = new JwtSecurityTokenHandler();

            // Convert the token string into a JwtSecurityToken object to access its properties.
            var jwtToken = handler.ReadJwtToken(token);

            // Attempt to extract the email claim value.
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

            return emailClaim;
        }
    }
}
