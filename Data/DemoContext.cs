using System.Data;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using PolyPort.Demo.Models;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;

namespace PolyPort.Demo.Data
{
    public class DemoContext : IDemoContext
    {
        private GremlinServer server;
        private GremlinClient client;
        public DemoContext() {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            
            string cosmosDBHost = GetDBHost(keyVaultClient).ConfigureAwait(false).GetAwaiter().GetResult().Value;
            string cosmosDBUsername = GetDBUsername(keyVaultClient).ConfigureAwait(false).GetAwaiter().GetResult().Value;
            string cosmosDBPassword = GetDBPassword(keyVaultClient).ConfigureAwait(false).GetAwaiter().GetResult().Value;

            this.server = new GremlinServer(
                    cosmosDBHost,
                    port: 443,
                    enableSsl: true, 
                    username: cosmosDBUsername, 
                    password: cosmosDBPassword);
            this.client = new GremlinClient(server, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
        }

        public async Task<ResultSet<dynamic>> SaveChanges(string query)
        {
            return await this.client.SubmitAsync<dynamic>(query);
        }

        public void Dispose() {
            this.client.Dispose();
        }

        private async Task<SecretBundle> GetDBHost(KeyVaultClient vaultClient) {
            Console.WriteLine("Get Host Secret");
            try {
                return await vaultClient.GetSecretAsync("https://polyportdemo.vault.azure.net/secrets/cosmosDBHostname/bbdd4312f4ec4cd0ba8c380991a81a61")
                     .ConfigureAwait(false);
            } 
            catch (KeyVaultErrorException keyVaultException)
            {
                Console.WriteLine(keyVaultException.Message);
                return null;
            }
        }

        private async Task<SecretBundle> GetDBUsername(KeyVaultClient vaultClient) {
            return await vaultClient.GetSecretAsync("https://polyportdemo.vault.azure.net/secrets/cosmosDBUsername")
                     .ConfigureAwait(false);
        }

        private async Task<SecretBundle> GetDBPassword(KeyVaultClient vaultClient) {
            return await vaultClient.GetSecretAsync("https://polyportdemo.vault.azure.net/secrets/cosmosDBPassword")
                     .ConfigureAwait(false);
        }
    }

    public interface IDemoContext
    {
        Task<ResultSet<dynamic>> SaveChanges(string query);
        void Dispose();
    }
}