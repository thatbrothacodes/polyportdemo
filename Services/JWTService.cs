
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using PolyPort.Demo.Models;

namespace PolyPort.Demo.Services {

    public interface IJWTService {
        bool ValidateToken(string token);
        Token DecodeToken(string jwt);
    }

    public class JWTService : IJWTService
    {
        JwtSecurityTokenHandler handler;
        public JWTService() {
            this.handler = new JwtSecurityTokenHandler();
        }
        public bool ValidateToken(string jwt)
        {
            return this.handler.CanReadToken(jwt);
        }

        public Token DecodeToken(string jwt) {
            if(this.ValidateToken(jwt)) {
                JwtSecurityToken jwtToken = this.handler.ReadJwtToken(jwt);

                return new Token {
                    iat = int.Parse(jwtToken.Payload["iat"].ToString()),
                    name = jwtToken.Payload["name"].ToString(),
                    sub = jwtToken.Payload["sub"].ToString()
                };
            }

            return null;
        }
    }
}
