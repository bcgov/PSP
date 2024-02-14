namespace Pims.Dal.Models
{
    public class PropertyOwnershipState
    {
        public bool isOwned { get; set; }

        public bool isOtherInterest { get; set; }

        public bool isPropertyOfInterest { get; set; }

        public bool isDisposed { get; set; }
    }
}
