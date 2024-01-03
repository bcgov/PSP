import moment from 'moment';

import { Api_PropertyFile } from '@/models/api/PropertyFile';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';
import { UtcIsoDateTime } from './UtcIsoDateTime';

export interface Api_File extends Api_ConcurrentVersion {
  id?: number;
  fileName?: string;
  fileNumber?: string;
  fileStatusTypeCode?: Api_TypeCode<string>;
  appCreateTimestamp?: UtcIsoDateTime;
  appLastUpdateTimestamp?: UtcIsoDateTime;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
  appCreateUserGuid?: string;
  appLastUpdateUserGuid?: string;
  fileProperties?: Api_PropertyFile[];
}

export interface Api_FileWithChecklist extends Api_File {
  fileChecklistItems: Api_FileChecklistItem[];
}

export interface Api_FileChecklistItem extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  fileId?: number;
  itemType?: Api_FileChecklistItemType;
  statusTypeCode?: Api_TypeCode<string>;
}

export interface Api_FileChecklistItemType extends Api_ConcurrentVersion {
  code?: string;
  sectionCode?: string;
  description?: string;
  hint?: string;
  isDisabled?: boolean;
  displayOrder?: number;
}

export function sortByDisplayOrder(a: Api_FileChecklistItem, b: Api_FileChecklistItem) {
  return (a.itemType?.displayOrder ?? 0) - (b.itemType?.displayOrder ?? 0);
}

export function lastModifiedBy(array: Api_AuditFields[] = []): Api_AuditFields | undefined {
  let lastModified: Api_AuditFields | undefined = undefined;

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

export function isDefaultState(checklistItem: Api_FileChecklistItem): boolean {
  return (
    checklistItem.statusTypeCode?.id === 'INCOMP' &&
    moment(checklistItem.appCreateTimestamp).isSame(
      moment(checklistItem.appLastUpdateTimestamp),
      'second',
    )
  );
}

export interface Api_LastUpdatedBy {
  parentId: number;
  appLastUpdateTimestamp: string | null;
  appLastUpdateUserid: string | null;
  appLastUpdateUserGuid: string | null;
}
