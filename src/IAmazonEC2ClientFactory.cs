using Amazon.EC2;

namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public interface IAmazonEC2ClientFactory
    {
        IAmazonEC2 Create();
    }
}