using System;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public class CloudFormationClient : ICloudFormationClient
    {
        public async Task Respond(string url, CloudFormationResponse response)
        {
            try
            {
                var client = new HttpClient();

                var jsonContent = new StringContent(JsonConvert.SerializeObject(response));
                jsonContent.Headers.Remove("Content-Type");

                var postResponse = await client.PutAsync(url, jsonContent);

                postResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                LambdaLogger.Log("Exception: " + ex);

                response.Status = "FAILED";
                response.Data = ex;
            }
        }
    }
}