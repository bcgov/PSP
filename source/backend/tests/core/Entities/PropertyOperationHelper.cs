using System;
using System.Linq;
using Pims.Api.Models.CodeTypes;
using Pims.Dal;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a property operation.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsPropertyOperation CreatePropertyOperation(long? propertyOperationId = null, PimsPropertyOperationType operationType = null, long operationNo = 1)
        {
            var propertyOperation = new Entity.PimsPropertyOperation()
            {
                PropertyOperationId = propertyOperationId ?? 1,
                ConcurrencyControlNumber = 1,
            };
            propertyOperation.PropertyOperationTypeCode = PropertyOperationTypes.SUBDIVIDE.ToString();
            propertyOperation.PropertyOperationTypeCodeNavigation = operationType ?? new Entity.PimsPropertyOperationType() { PropertyOperationTypeCode = PropertyOperationTypes.SUBDIVIDE.ToString(), Description = "SUBDIVIDE", DbCreateUserid = "create user", DbLastUpdateUserid = "last user" };
            propertyOperation.PropertyOperationNo = operationNo;
            propertyOperation.OperationDt = DateTime.UtcNow;
            propertyOperation.SourceProperty = EntityHelper.CreateProperty(1, isCoreInventory: true);
            propertyOperation.DestinationProperty = EntityHelper.CreateProperty(2, isCoreInventory: true);

            return propertyOperation;
        }

        /// <summary>
        /// Create a new instance of a property operation.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsPropertyOperation CreatePropertyOperation(this PimsContext context, long? propertyOperationId = null)
        {
            var operationType = context.PimsPropertyOperationTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find property operation type.");
            var propertyOperation = EntityHelper.CreatePropertyOperation(propertyOperationId: propertyOperationId, operationType: operationType);
            context.PimsPropertyOperations.Add(propertyOperation);

            return propertyOperation;
        }
    }
}
