using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PolyPort.Demo.Data;
using PolyPort.Demo.Repositories;
using PolyPort.Demo.Services;

[assembly: FunctionsStartup(typeof(PolyPort.Demo.Startup))]

namespace PolyPort.Demo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IDemoContext, DemoContext>();
            builder.Services.AddTransient<ITokenRepository, TokenRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IJWTService, JWTService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
        }
    }
}