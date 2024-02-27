import moment from 'moment';

import { ApiGen_Base_BaseAudit } from '@/models/api/generated/ApiGen_Base_BaseAudit';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';

export function sortByDisplayOrder(
  a: ApiGen_Concepts_FileChecklistItem,
  b: ApiGen_Concepts_FileChecklistItem,
) {
  return (a.itemType?.displayOrder ?? 0) - (b.itemType?.displayOrder ?? 0);
}

export function lastModifiedBy(
  array: ApiGen_Base_BaseAudit[] = [],
): ApiGen_Base_BaseAudit | undefined {
  let lastModified: ApiGen_Base_BaseAudit | undefined = undefined;

  for (const item of array) {
    if (moment(item.appLastUpdateTimestamp).isBefore(moment('1900-01-01T00:00:00.000Z'))) {
      continue;
    }
    if (lastModified === undefined) {
      lastModified = item;
      continue;
    }
    if (moment(item.appLastUpdateTimestamp).isAfter(moment(lastModified.appLastUpdateTimestamp))) {
      lastModified = item;
    }
  }

  return lastModified;
}

export function isDefaultState(checklistItem: ApiGen_Concepts_FileChecklistItem): boolean {
  return (
    checklistItem.statusTypeCode?.id === 'INCOMP' &&
    moment(checklistItem.appCreateTimestamp).isSame(
      moment(checklistItem.appLastUpdateTimestamp),
      'second',
    )
  );
}
