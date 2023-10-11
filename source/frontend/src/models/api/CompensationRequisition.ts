import {
  Api_AcquisitionFile,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFilePerson,
} from './AcquisitionFile';
import { Api_AuditFields } from './AuditFields';
import { Api_CompensationFinancial } from './CompensationFinancial';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_FinancialCode } from './FinancialCode';
import { Api_InterestHolder } from './InterestHolder';
import { Api_Project } from './Project';

export interface Api_CompensationRequisition extends Api_ConcurrentVersion, Api_AuditFields {
  id: number | null;
  acquisitionFileId: number;
  acquisitionFile: Api_AcquisitionFile | null;
  alternateProject: Api_Project | null;
  alternateProjectId: number | null;
  isDraft: boolean | null;
  fiscalYear: string | null;
  yearlyFinancialId: number | null;
  yearlyFinancial: Api_FinancialCode | null;
  chartOfAccountsId: number | null;
  chartOfAccounts: Api_FinancialCode | null;
  responsibilityId: number | null;
  responsibility: Api_FinancialCode | null;
  finalizedDate: string | null;
  agreementDate: string | null;
  expropriationNoticeServedDate: string | null;
  expropriationVestingDate: string | null;
  advancedPaymentServedDate: string | null;
  generationDate: string | null;
  financials: Api_CompensationFinancial[];
  acquisitionOwnerId: number | null;
  acquisitionOwner: Api_AcquisitionFileOwner | null;
  interestHolderId: number | null;
  interestHolder: Api_InterestHolder | null;
  acquisitionFilePersonId: number | null;
  acquisitionFilePerson: Api_AcquisitionFilePerson | null;
  legacyPayee: string | null;
  isPaymentInTrust: boolean | null;
  gstNumber: string | null;
  specialInstruction: string | null;
  detailedRemarks: string | null;
  isDisabled: boolean | null;
}
