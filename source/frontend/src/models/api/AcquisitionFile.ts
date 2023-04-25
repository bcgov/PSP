import { Api_File } from 'models/api/File';
import Api_TypeCode from 'models/api/TypeCode';
import moment from 'moment';

import { Api_Address } from './Address';
import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Person } from './Person';
import { Api_Product, Api_Project } from './Project';
import { Api_PropertyFile } from './PropertyFile';

export interface Api_AcquisitionFile extends Api_ConcurrentVersion, Api_AuditFields, Api_File {
  id?: number;
  fileNo?: number;
  legacyFileNumber?: string;
  assignedDate?: string;
  deliveryDate?: string;
  completionDate?: string;
  // Code Tables
  acquisitionPhysFileStatusTypeCode?: Api_TypeCode<string>;
  acquisitionTypeCode?: Api_TypeCode<string>;
  // MOTI region
  regionCode?: Api_TypeCode<number>;
  acquisitionTeam?: Api_AcquisitionFilePerson[];
  acquisitionFileOwners?: Api_AcquisitionFileOwner[];
  acquisitionFileChecklist?: Api_AcquisitionFileChecklistItem[];
  acquisitionFileOwnerSolicitors?: Api_AcquisitionFileSolicitor[];

  project?: Api_Project;
  product?: Api_Product;
  fundingTypeCode?: Api_TypeCode<string>;
  fundingOther?: string;
}

export interface Api_AcquisitionFileProperty
  extends Api_ConcurrentVersion,
    Api_PropertyFile,
    Api_AuditFields {}

export interface Api_AcquisitionFilePerson extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  personId?: number;
  person?: Api_Person;
  personProfileTypeCode?: string;
  personProfileType?: Api_TypeCode<string>;
  isDisabled?: boolean;
}

export interface Api_AcquisitionFileSolicitor extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  personId?: number;
  person?: Api_Person;
  isDisabled?: boolean;
}

export interface Api_AcquisitionFileOwner extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  acquisitionFileId?: number;
  isPrimaryContact: boolean;
  isOrganization: boolean;
  lastNameAndCorpName: string | null;
  otherName: string | null;
  givenName: string | null;
  incorporationNumber: string | null;
  registrationNumber: string | null;
  contactEmailAddr: string | null;
  contactPhoneNum: string | null;
  address: Api_Address | null;
}

export interface Api_AcquisitionFileChecklistItem extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  acquisitionFileId?: number;
  itemType?: Api_AcquisitionFileChecklistItemType;
  statusTypeCode?: Api_TypeCode<string>;
}

export interface Api_AcquisitionFileChecklistItemType extends Api_ConcurrentVersion {
  code?: string;
  sectionCode?: string;
  description?: string;
  hint?: string;
  isDisabled?: boolean;
  displayOrder?: number;
}

export function sortByDisplayOrder(
  a: Api_AcquisitionFileChecklistItem,
  b: Api_AcquisitionFileChecklistItem,
) {
  return (a.itemType?.displayOrder ?? 0) - (b.itemType?.displayOrder ?? 0);
}

export function lastModifiedBy(array: Api_AuditFields[] = []): Api_AuditFields | undefined {
  let lastModified: Api_AuditFields | undefined = undefined;

  for (const item of array) {
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

export function isDefaultState(checklistItem: Api_AcquisitionFileChecklistItem): boolean {
  return (
    checklistItem.statusTypeCode?.id === 'INCOMP' &&
    moment(checklistItem.appCreateTimestamp).isSame(
      moment(checklistItem.appLastUpdateTimestamp),
      'second',
    )
  );
}
