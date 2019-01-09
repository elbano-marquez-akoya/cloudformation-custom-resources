namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public interface IRequest
    {
        string StackId { get; set; }
        string ResponseURL { get; set; }
        string RequestId { get; set; }
        string LogicalResourceId { get; set; }
        string PhysicalResourceId { get; set; }
    }
}