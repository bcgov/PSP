using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IPropertyOperationService
    {
        public IEnumerable<PimsPropertyOperation> SubdivideProperty(IEnumerable<PimsPropertyOperation> operations);
    }
}
