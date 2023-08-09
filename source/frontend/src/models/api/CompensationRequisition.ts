import { Api_AuditFields } from './AuditFields';
import { Api_CompensationFinancial } from './CompensationFinancial';
import { Api_CompensationPayee } from './CompensationPayee';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_FinancialCode } from './FinancialCode';

export interface Api_CompensationRequisition extends Api_ConcurrentVersion, Api_AuditFields {
  id: number | null;
  acquisitionFileId: number;
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
  generationDate: string | null;
  specialInstruction: string | null;
  detailedRemarks: string | null;
  isDisabled: boolean | null;
  financials: Api_CompensationFinancial[];
  payees: Api_CompensationPayee[];
}
