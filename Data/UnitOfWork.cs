using Gremlin.Net.Driver;
using PolyPort.Demo.Repositories;
using System;

namespace PolyPort.Demo.Data {
    public interface IUnitOfWork
    {
        ITokenRepository TokenRepository { get; }
        void Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDemoContext _context;
        private ITokenRepository _tokenRepository;
        public UnitOfWork(IDemoContext context)
        {
            this._context = context;
        }

        public ITokenRepository TokenRepository
        {
            get
            {
                return this._tokenRepository = this._tokenRepository ?? new TokenRepository(this._context);
            }
        }

        public void Save()
        {
            ;
        }
    }
}