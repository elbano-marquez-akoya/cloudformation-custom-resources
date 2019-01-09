using System.Threading.Tasks;

namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public interface ICloudFormationClient
    {
        Task Respond(string url, CloudFormationResponse response);
    }
}