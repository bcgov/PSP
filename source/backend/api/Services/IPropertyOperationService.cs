using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IPropertyOperationService
    {
        IList<PimsPropertyOperation> GetOperationsForProperty(long propertyId);

        IEnumerable<PimsPropertyOperation> SubdivideProperty(IEnumerable<PimsPropertyOperation> operations);
    }
}
