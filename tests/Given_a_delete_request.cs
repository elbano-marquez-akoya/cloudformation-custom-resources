using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using NSubstitute;
using NUnit.Framework;

namespace Ehl.TransitGateway.CloudFormation.CustomResource.Tests
{
    [TestFixture]
    public class Given_a_delete_request
    {
        private FunctionFixture _fixture;

        [SetUp]
        public async Task SetUp()
        {
            _fixture = new FunctionFixture();
            await _fixture.Execute("delete-request.json");
        }

        [Test]
        public async Task Deletes_the_route_with_matching_cidr_block()
        {
            await RouteDeletedWith(r => r.DestinationCidrBlock == "the-destination-cidr-block-for-delete");
        }

        [Test]
        public async Task Deletes_the_route_with_matching_route_table_id()
        {
            await RouteDeletedWith(r => r.RouteTableId == "the-route-table-id-for-delete");
        }

        private async Task RouteDeletedWith(Expression<Predicate<DeleteRouteRequest>> predicate)
        {
            await _fixture.EC2.Received(1).DeleteRouteAsync(Arg.Is(predicate));
        }

        [Test]
        public async Task Responds_with_success()
        {
            await RespondsWith(r => r.Status == "SUCCESS" && r.Reason == "");
        }

        [Test]
        public async Task Responds_with_the_physical_resource_id()
        {
            await RespondsWith(r => r.PhysicalResourceId == "the-physical-resource-id-for-delete");
        }

        [Test]
        public async Task Responds_with_the_stack_id()
        {
            await RespondsWith(r => r.StackId == "the-stack-id-for-delete");
        }

        [Test]
        public async Task Responds_with_the_request_id()
        {
            await RespondsWith(r => r.RequestId == "the-request-id-for-delete");
        }

        [Test]
        public async Task Responds_with_the_logical_resource_id()
        {
            await RespondsWith(r => r.LogicalResourceId == "the-logical-resource-id-for-delete");
        }

        private async Task RespondsWith(Expression<Predicate<CloudFormationResponse>> predicate)
        {
            await _fixture.CloudFormation.Received(1).Respond("cloud-formation-response-url-for-delete", Arg.Is(predicate));
        }
    }
}