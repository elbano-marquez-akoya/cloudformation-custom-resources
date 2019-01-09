using Amazon.EC2;

namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public class AmazonEC2Factory : IAmazonEC2ClientFactory
    {
        public IAmazonEC2 Create() => new AmazonEC2Client();
    }
}