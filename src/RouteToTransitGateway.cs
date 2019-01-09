namespace Ehl.TransitGateway.CloudFormation.CustomResource
{
    public class RouteToTransitGateway
    {
        public string DestinationCidrBlock { get; set; }
        public string RouteTableId { get; set; }
        public string TransitGatewayId { get; set; }

        public override string ToString()
        {
            return $"{RouteTableId}::{DestinationCidrBlock}";
        }
    }
}