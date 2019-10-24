using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolyPort.Demo.Services;
using System.Net.Http;
using System.Net;
using Gremlin.Net.Driver;

namespace Polyport.Demo
{
    public class TokenController
    {
        private readonly IJWTService _jwtService;
        private readonly ITokenService _tokenService;

        public TokenController(IJWTService jwtService, ITokenService tokenService) {
            this._jwtService = jwtService;
            this._tokenService = tokenService;
        }

        [FunctionName("GetToken")]
        public async Task<HttpResponseMessage> GetToken([HttpTrigger(AuthorizationLevel.Function, "get", Route = "token")] HttpRequestMessage req,
            ILogger log) {
                try {
                    var results = await _tokenService.GetToken();
                    return req.CreateResponse(HttpStatusCode.OK, results);
                }
                catch(Exception e) {
                    Console.WriteLine(e);
                    return req.CreateResponse(HttpStatusCode.InternalServerError);
                }
        }

        [FunctionName("CreateToken")]
        public async Task<HttpResponseMessage> CreateToken(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "token/{jsonWebToken?}")] HttpRequestMessage req,
            string jsonWebToken,
            ILogger log)
        {
            string bearerToken = (req.Headers.Authorization != null) ? req.Headers.Authorization.ToString() : string.Empty;
            string jwt;
            ResultSet<dynamic> token;

            if(bearerToken != null && bearerToken != string.Empty) {
                jwt = bearerToken.Substring(7);
            } 
            else {
                jwt = jsonWebToken;
            }

            token = await this._tokenService.AddToken(jwt);

            if (token != null)
            {
                return req.CreateResponse(HttpStatusCode.Created, token);
            }

            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
