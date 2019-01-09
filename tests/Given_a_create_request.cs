using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using NSubstitute;
using NUnit.Framework;

namespace Ehl.TransitGateway.CloudFormation.CustomResource.Tests
{
    [TestFixture]
    public class Given_a_create_request
    {
        private FunctionFixture _fixture;

        [SetUp]
        public async Task SetUp()
        {
            _fixture = new FunctionFixture();
            await _fixture.Execute("create-request.json");
        }

        [Test]
        public async Task Creates_a_route_with_the_destination_cidr_block()
        {
            await RouteCreatedWith(r => r.DestinationCidrBlock == "the-destination-cidr-block-for-create");
        }

        [Test]
        public async Task Creates_a_route_with_the_transit_gateway_id()
        {
            await RouteCreatedWith(r => r.TransitGatewayId == "the-transit-gateway-id-for-create");
        }

        [Test]
        public async Task Creates_a_route_with_the_route_table_id()
        {
            await RouteCreatedWith(r => r.RouteTableId == "the-route-table-id-for-create");
        }

        private async Task RouteCreatedWith(Expression<Predicate<CreateRouteRequest>> predicate)
        {
            await _fixture.EC2.Received(1).CreateRouteAsync(Arg.Is(predicate));
        }

        [Test]
        public async Task Responds_with_success()
        {
            await RespondsWith(r => r.Status == "SUCCESS" && r.Reason == "");
        }

        [Test]
        public async Task Responds_with_the_physical_resource_id()
        {
            await RespondsWith(r => r.PhysicalResourceId ==
                                    "the-route-table-id-for-create::the-destination-cidr-block-for-create");
        }

        [Test]
        public async Task Responds_with_the_stack_id()
        {
            await RespondsWith(r => r.StackId == "the-stack-id-for-create");
        }

        [Test]
        public async Task Responds_with_the_request_id()
        {
            await RespondsWith(r => r.RequestId == "the-request-id-for-create");
        }

        [Test]
        public async Task Responds_with_the_logical_resource_id()
        {
            await RespondsWith(r => r.LogicalResourceId == "the-logical-resource-id-for-create");
        }

        private async Task RespondsWith(Expression<Predicate<CloudFormationResponse>> predicate)
        {
            await _fixture.CloudFormation.Received(1).Respond("cloud-formation-response-url-for-create", Arg.Is(predicate));
        }
    }
}