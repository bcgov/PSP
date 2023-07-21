import { Api_AcquisitionFileProperty } from './AcquisitionFile';
import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion, Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import Api_TypeCode from './TypeCode';

export interface Api_InterestHolder extends Api_ConcurrentVersion, Api_AuditFields {
  interestHolderId: number | null;
  interestHolderType: Api_TypeCode<string> | null;
  acquisitionFileId: number | null;
  personId: number | null;
  person: Api_Person | null;
  organizationId: number | null;
  organization: Api_Organization | null;
  interestHolderProperties: Api_InterestHolderProperty[];
  primaryContactId: number | null;
  primaryContact: Api_Person | null;
  comment: string | null;
  isDisabled: boolean;
}

export interface Api_InterestHolderProperty extends Api_ConcurrentVersion_Null, Api_AuditFields {
  interestHolderPropertyId: number | null;
  interestHolderId: number | null;
  acquisitionFileProperty: Api_AcquisitionFileProperty | null;
  acquisitionFilePropertyId: number | null;
  propertyInterestTypes: Api_TypeCode<string>[];
  isDisabled: boolean;
}
