using Newtonsoft.Json;
using RestSharp;

namespace ApiTester;

public class Test
{
    public static void Main(string[] args)
    {
        new Test().Call();
    }

    public async void Call()
    {
        //https://api.github.com/repos/OWNER/REPO/deployments

        try
        {
            var options = new RestClientOptions("https://api.github.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = 5_000
            };

            var client = new RestClient(options);
            var deploymentId = GetIdOfLatestDeployment(client);
            GetLatestDeploymentData(client, deploymentId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.ReadKey();
        }
    }


}