using System;
using System.Linq;
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
        /// Create a new instance of an Disposition File checklist item.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsDispositionChecklistItem CreateDispositionChecklistItem(long? id = null, long? dispositionFileId = null, PimsChklstItemStatusType statusType = null, PimsDspChklstItemType itemType = null)
        {
            var checklistItem = new Entity.PimsDispositionChecklistItem()
            {
                Internal_Id = id ?? 1,
                DispositionFileId = dispositionFileId ?? 1,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
            checklistItem.ChklstItemStatusTypeCodeNavigation = statusType ?? new Entity.PimsChklstItemStatusType() { Id = "INCOMP", Description = "Incomplete", DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now };
            checklistItem.ChklstItemStatusTypeCode = checklistItem.ChklstItemStatusTypeCodeNavigation.Id;
            checklistItem.DspChklstItemTypeCodeNavigation = itemType ?? new Entity.PimsDspChklstItemType() { Id = "APPRAISE", Description = "Appraisals and reviews", DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now, DspChklstSectionTypeCode = "section" };
            checklistItem.DspChklstItemTypeCode = checklistItem.DspChklstItemTypeCodeNavigation.Id;

            return checklistItem;
        }

        /// <summary>
        /// Create a new instance of an Disposition File checklist item.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsDispositionChecklistItem CreateDispositionChecklistItem(this PimsContext context, long? id = null, long? dispositionFileId = null)
        {
            var statusType = context.PimsChklstItemStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find checklist item status type.");
            var itemType = context.PimsDspChklstItemTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find checklist item type.");
            var checklistItem = EntityHelper.CreateDispositionChecklistItem(id: id, dispositionFileId: dispositionFileId, statusType: statusType, itemType: itemType);
            context.PimsDispositionChecklistItems.Add(checklistItem);

            return checklistItem;
        }
    }
}
