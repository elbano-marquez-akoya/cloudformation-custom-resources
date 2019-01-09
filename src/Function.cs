using System;
using System.Threading.Tasks;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public class Function
    {
        private readonly IAmazonEC2ClientFactory _ec2ClientFactory;
        private readonly ICloudFormationClient _cfClient;

        public Function() : this(new AmazonEC2Factory(), new CloudFormationClient())
        {
        }

        public Function(IAmazonEC2ClientFactory ec2ClientFactory, ICloudFormationClient cfClient)
        {
            _ec2ClientFactory = ec2ClientFactory;
            _cfClient = cfClient;
        }

        public async Task<string> FunctionHandler(CloudFormationRequest<RouteToTransitGateway> input)
        {
            var physicalResourceId = "unassigned-physical-resource-id";

            try
            {
                using (var client = _ec2ClientFactory.Create())
                {
                    switch (input.RequestType)
                    {
                        case "Create":
                            physicalResourceId = input.ResourceProperties.ToString();
                            await CreateRoute(client, input.ResourceProperties);
                            await Respond(input, physicalResourceId);
                            break;
                        case "Delete":
                            physicalResourceId = input.PhysicalResourceId;
                            await DeleteRoute(client, input.ResourceProperties);
                            await Respond(input, physicalResourceId);
                            break;
                        case "Update":
                            physicalResourceId = input.ResourceProperties.ToString();
                            await DeleteRoute(client, input.OldResourceProperties);
                            await CreateRoute(client, input.ResourceProperties);
                            await Respond(input, physicalResourceId);
                            break;
                        default:
                            throw new NotSupportedException($"Cannot handle RequestType {input.RequestType}");
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                await Respond(input, physicalResourceId, ex);
                LambdaLogger.Log("Exception: " + ex);
                return $"FAILED: {ex.GetType().Name} {ex.Message}";
            }
        }

        private async Task CreateRoute(IAmazonEC2 client, RouteToTransitGateway route)
        {
            var request = new CreateRouteRequest
            {
                DestinationCidrBlock = route.DestinationCidrBlock,
                TransitGatewayId = route.TransitGatewayId,
                RouteTableId = route.RouteTableId
            };

            await client.CreateRouteAsync(request);
        }

        private async Task DeleteRoute(IAmazonEC2 client, RouteToTransitGateway route)
        {
            var request = new DeleteRouteRequest
            {
                DestinationCidrBlock = route.DestinationCidrBlock,
                RouteTableId = route.RouteTableId
            };

            await client.DeleteRouteAsync(request);
        }

        private async Task Respond(IRequest request, string physicalResourceId, Exception exception = null)
        {
            var responseBody = new CloudFormationResponse
            {
                Status = exception != null ? "FAILED" : "SUCCESS",
                Reason = exception?.Message ?? string.Empty,
                PhysicalResourceId = physicalResourceId,
                StackId = request.StackId,
                RequestId = request.RequestId,
                LogicalResourceId = request.LogicalResourceId,
            };

            await _cfClient.Respond(request.ResponseURL, responseBody);
        }
    }
}