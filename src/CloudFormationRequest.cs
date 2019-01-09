namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public class CloudFormationRequest<T> : IRequest
    {
        public string StackId { get; set; }
        public string ResponseURL { get; set; }
        public string RequestType { get; set; }
        public string ResourceType { get; set; }
        public string RequestId { get; set; }
        public string LogicalResourceId { get; set; }
        public string PhysicalResourceId { get; set; }
        public T ResourceProperties { get; set; }
        public T OldResourceProperties { get; set; }
    }
}