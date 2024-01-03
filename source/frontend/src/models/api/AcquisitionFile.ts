import { Api_FileWithChecklist } from '@/models/api/File';
import Api_TypeCode from '@/models/api/TypeCode';

import { Api_Address } from './Address';
import { Api_AuditFields } from './AuditFields';
import { Api_CompensationRequisition } from './CompensationRequisition';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { DateOnly } from './DateOnly';
import { Api_InterestHolder } from './InterestHolder';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import { Api_Product, Api_Project } from './Project';
import { Api_PropertyFile } from './PropertyFile';
import { UtcIsoDateTime } from './UtcIsoDateTime';

export enum EnumAcquisitionFileType {
  CONSEN = 'CONSEN',
  SECTN3 = 'SECTN3',
  SECTN6 = 'SECTN6',
}

// LINK @backend/api/Models/Concepts/AcquisitionFile/AcquisitionFileModel.cs
export interface Api_AcquisitionFile
  extends Api_ConcurrentVersion,
    Api_AuditFields,
    Api_FileWithChecklist {
  id?: number;
  fileNo?: number;
  legacyFileNumber?: string;
  assignedDate?: UtcIsoDateTime;
  deliveryDate?: DateOnly;
  completionDate?: DateOnly;
  // Code Tables
  acquisitionPhysFileStatusTypeCode?: Api_TypeCode<string>;
  acquisitionTypeCode?: Api_TypeCode<string>;
  // MOTI region
  regionCode?: Api_TypeCode<number>;
  acquisitionTeam?: Api_AcquisitionFileTeam[];
  acquisitionFileOwners?: Api_AcquisitionFileOwner[];
  acquisitionFileInterestHolders?: Api_InterestHolder[];
  compensationRequisitions?: Api_CompensationRequisition[];
  project?: Api_Project;
  projectId: number | null;
  product?: Api_Product;
  productId: number | null;
  fundingTypeCode?: Api_TypeCode<string>;
  fundingOther?: string;
  totalAllowableCompensation?: number;
  legacyStakeholders?: string[] | null;
}

export interface Api_AcquisitionFileProperty
  extends Api_ConcurrentVersion,
    Api_PropertyFile,
    Api_AuditFields {}

export interface Api_AcquisitionFileTeam extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  acquisitionFileId: number;
  personId?: number;
  person?: Api_Person;
  organizationId?: number;
  organization?: Api_Organization;
  primaryContactId?: number;
  primaryContact?: Api_Person;
  teamProfileTypeCode?: string;
  teamProfileType?: Api_TypeCode<string>;
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
