import {
  Api_AcquisitionFile,
  Api_AcquisitionFileRepresentative,
  Api_AcquisitionFileSolicitor,
} from './AcquisitionFile';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import Api_TypeCode from './TypeCode';

export interface Api_CompensationFinancial extends Api_ConcurrentVersion {
  id: number;
  compensationId: number;
  pretaxAmount: number | null;
  taxAmount: number | null;
  totalAmount: number | null;
  isDisabled: boolean | null;
  financialActivityCode: Api_TypeCode<number> | null;
}

export interface Api_CompensationPayee extends Api_ConcurrentVersion {
  id: number;
  compensationRequisitionId: number;
  acquisitionOwnerId: number | null;
  interestHolderId: number | null;
  ownerRepresentativeId: number | null;
  ownerSolicitorId: number | null;
  motiSolicitorId: number | null;
  isDisabled: boolean | null;
  motiSolicitor: Api_Person | null;
  acquisitionOwner: Api_AcquisitionFile | null;
  compensationRequisition: Api_Compensation | null;
  interestHolder: Api_InterestHolder | null;
  ownerRepresentative: Api_AcquisitionFileRepresentative | null;
  ownerSolicitor: Api_AcquisitionFileSolicitor | null;
}

export interface Api_InterestHolder extends Api_ConcurrentVersion {
  id: number;
  personId?: number;
  organizationId?: number;
  isDisabled?: boolean;
  acquisitionFileId: number;
  acquisitionFile: Api_AcquisitionFile;
  organization: Api_Organization;
  person: Api_Person;
}

export interface Api_Compensation extends Api_ConcurrentVersion {
  id: number | null;
  acquisitionFileId: number;
  isDraft: boolean | null;
  fiscalYear: string | null;
  agreementDate: string | null;
  expropriationNoticeServedDate: string | null;
  expropriationVestingDate: string | null;
  generationDate: string | null;
  specialInstruction: string | null;
  detailedRemarks: string | null;
  isDisabled: boolean | null;
  financials: Api_CompensationFinancial[];
  payees: Api_CompensationPayee[];
}
