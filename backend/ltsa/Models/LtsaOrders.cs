using System.Collections.Generic;

namespace Pims.Ltsa.Models
{
    public class LtsaOrders
    {
        public OrderParent<ParcelInfo> ParcelInfo { get; set; }
        public IEnumerable<OrderParent<Title>> TitleOrders { get; set; }
    }
}
