using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Ehl.TransitGateway.CloudFormation.CustomResource.Tests
{
    [TestFixture]
    public class Given_a_create_request_which_fails
    {
        private FunctionFixture _fixture;

        [SetUp]
        public async Task SetUp()
        {
            _fixture = new FunctionFixture();
            _fixture.EC2.CreateRouteAsync(Arg.Any<CreateRouteRequest>()).Throws(new Exception("the-exception-message"));
            await _fixture.Execute("create-request.json");
        }

        [Test]
        public async Task Responds_with_failure()
        {
            await RespondsWith(r => r.Status == "FAILED");
        }

        [Test]
        public async Task Responds_with_reason_for_failure()
        {
            await RespondsWith(r => r.Reason == "the-exception-message");
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