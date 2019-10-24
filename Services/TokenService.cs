using System.Threading.Tasks;
using Gremlin.Net.Driver;
using PolyPort.Demo.Data;
using PolyPort.Demo.Models;

namespace PolyPort.Demo.Services {

    public interface ITokenService {
        Task<ResultSet<dynamic>> GetToken();
        Task<ResultSet<dynamic>> AddToken(string jwt);
    }

    public class TokenService : ITokenService
    {
        IUnitOfWork _uow;
        IJWTService _service;
        public Task<ResultSet<dynamic>> GetToken()
        {
            return this._uow.TokenRepository.GetAllAsync();
        }

        public Task<ResultSet<dynamic>> AddToken(string jwt)
        {
            Token token = this._service.DecodeToken(jwt);
            
            if(token != null) {
                return this._uow.TokenRepository.AddAsync(token);
            }
            
            return null;
        }

        public TokenService(IUnitOfWork uow, IJWTService service) {
            this._uow = uow;
            this._service = service;
        }
    }
}
