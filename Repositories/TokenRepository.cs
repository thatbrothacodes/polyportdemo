using System;
using System.Threading.Tasks;
using PolyPort.Demo.Data;
using Gremlin.Net.Driver;
using PolyPort.Demo.Models;

namespace PolyPort.Demo.Repositories {
    public class TokenRepository : GenericRepository<Token>, ITokenRepository 
    {
        public override async Task<ResultSet<dynamic>> AddAsync(Token t) {
            string query = string.Format(@"
                g.addV()
                .property('label', 'token')
                .property('pk', '')
                .property('iat', {0})
                .property('name', '{1}')
                .property('sub', '{2}')", t.iat, t.name, t.sub);
            return await this._context.SaveChanges(query);
        }
        public TokenRepository(IDemoContext context): base(context, "tokens") {}
    }

    public interface ITokenRepository : IGenericRepository<Token> {

    }
}