using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");

            var response = await tokenClient.RequestClientCredentialsAsync("api1");
            //System.Console.WriteLine(response.AccessToken);

            var client = new HttpClient();
            client.SetBearerToken(response.AccessToken);

            var data = await client.GetStringAsync("http://localhost:5002/test");
            System.Console.WriteLine(data);
        }
    }
}