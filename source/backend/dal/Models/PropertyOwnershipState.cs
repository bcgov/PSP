namespace Pims.Dal.Models
{
    public class PropertyOwnershipState
    {
        public bool isOwned { get; set; } = false;

        public bool isOtherInterest { get; set; } = false;

        public bool isPropertyOfInterest { get; set; } = false;

        public bool isDisposed { get; set; } = false;
    }
}
