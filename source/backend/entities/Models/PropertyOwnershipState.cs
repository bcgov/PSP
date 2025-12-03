namespace Pims.Dal.Models
{
    public class PropertyOwnershipState
    {
        public bool IsOwned { get; set; } = false;

        public bool IsOtherInterest { get; set; } = false;

        public bool IsPropertyOfInterest { get; set; } = false;

        public bool IsDisposed { get; set; } = false;
    }
}
