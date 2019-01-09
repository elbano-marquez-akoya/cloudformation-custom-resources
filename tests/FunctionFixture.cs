using System.Threading.Tasks;
using Amazon.EC2;
using Amazon.Lambda.Serialization.Json;
using NSubstitute;

namespace Ehl.TransitGateway.CloudFormation.CustomResource.Tests
{
    internal class FunctionFixture
    {
        public IAmazonEC2 EC2 { get; }
        public ICloudFormationClient CloudFormation { get; }

        public FunctionFixture()
        {
            EC2 = Substitute.For<IAmazonEC2>();
            CloudFormation = Substitute.For<ICloudFormationClient>();
        }

        public async Task Execute(string jsonRequest)
        {
            var ec2ClientFactory = Substitute.For<IAmazonEC2ClientFactory>();
            ec2ClientFactory.Create().Returns(EC2);

            var function = new Function(ec2ClientFactory, CloudFormation);

            var request = CreateRequest(jsonRequest);

            await function.FunctionHandler(request);
        }

        private CloudFormationRequest<RouteToTransitGateway> CreateRequest(string name)
        {
            var type = GetType();
            var embeddedResourceName = $"{type.Namespace}.{name}";
            using (var createJson = type.Assembly.GetManifestResourceStream(embeddedResourceName))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<CloudFormationRequest<RouteToTransitGateway>>(createJson);
            }
        }
    }
}