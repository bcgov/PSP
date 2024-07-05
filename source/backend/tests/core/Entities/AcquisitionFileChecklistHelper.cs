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
        /// Create a new instance of an Acquisition File checklist item.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsAcquisitionChecklistItem CreateAcquisitionChecklistItem(long? id = null, long? acquisitionFileId = null, PimsChklstItemStatusType statusType = null, PimsAcqChklstItemType itemType = null)
        {
            var checklistItem = new Entity.PimsAcquisitionChecklistItem()
            {
                Internal_Id = id ?? 1,
                AcquisitionFileId = acquisitionFileId ?? 1,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
            checklistItem.ChklstItemStatusTypeCodeNavigation = statusType ?? new Entity.PimsChklstItemStatusType() { Id = ChecklistItemStatusTypes.INCOMP.ToString(), Description = "Incomplete", DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now };
            checklistItem.ChklstItemStatusTypeCode = checklistItem.ChklstItemStatusTypeCodeNavigation.Id;
            checklistItem.AcqChklstItemTypeCodeNavigation = itemType ?? new Entity.PimsAcqChklstItemType() { Id = "APPRAISE", Description = "Appraisals and reviews", DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now, AcqChklstSectionTypeCode = "section" };
            checklistItem.AcqChklstItemTypeCode = checklistItem.AcqChklstItemTypeCodeNavigation.Id;

            return checklistItem;
        }

        /// <summary>
        /// Create a new instance of an Acquisition File checklist item.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsAcquisitionChecklistItem CreateAcquisitionChecklistItem(this PimsContext context, long? id = null, long? acquisitionFileId = null)
        {
            var statusType = context.PimsChklstItemStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find checklist item status type.");
            var itemType = context.PimsAcqChklstItemTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find checklist item type.");
            var checklistItem = EntityHelper.CreateAcquisitionChecklistItem(id: id, acquisitionFileId: acquisitionFileId, statusType: statusType, itemType: itemType);
            context.PimsAcquisitionChecklistItems.Add(checklistItem);

            return checklistItem;
        }
    }
}
