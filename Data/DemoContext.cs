using System.Data;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using PolyPort.Demo.Models;
using System;

namespace PolyPort.Demo.Data
{
    public class DemoContext : IDemoContext
    {
        private GremlinServer server;
        private GremlinClient client;
        public DemoContext() {
            this.server = new GremlinServer(
                    "polyportdbuser.gremlin.cosmos.azure.com", port: 443,
                    enableSsl: true, 
                    username: "/dbs/polyport.demo/colls/tokens", 
                    password: "MRzDTRPUnLDTW02Rf2a6cvj9TWQPMtcHfJs16sYUP1j6jarSfmbSjK0OTZpumrRSMIrWQmkvgdeBXfg8nBoqeQ==");
            this.client = new GremlinClient(server, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
        }

        public async Task<ResultSet<dynamic>> SaveChanges(string query)
        {
            return await this.client.SubmitAsync<dynamic>(query);
        }

        public void Dispose() {
            this.client.Dispose();
        }
    }

    public interface IDemoContext
    {
        Task<ResultSet<dynamic>> SaveChanges(string query);
        void Dispose();
    }
}