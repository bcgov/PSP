using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IManagementActivityService
    {
        Paged<PimsManagementActivity> GetPage(ManagementActivityFilter filter, bool? all = false);

        IList<PimsManagementActivity> SearchManagementActivities(ManagementActivityFilter filter);

        IList<PimsManagementActivityInvoice> SearchManagementActivityInvoices(ManagementActivityFilter filter);
    }
}
