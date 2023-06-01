import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion, Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { Api_FinancialCode } from './FinancialCode';
import { Api_Payee } from './Payee';
import Api_TypeCode from './TypeCode';

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
  agreementDate: string | null;
  expropriationNoticeServedDate: string | null;
  expropriationVestingDate: string | null;
  generationDate: string | null;
  specialInstruction: string | null;
  detailedRemarks: string | null;
  isDisabled: boolean | null;
  financials: Api_CompensationFinancial[] | null;
  payees: Api_Payee[] | null;
}

export interface Api_CompensationFinancial extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  compensationId: number;
  financialActivityCodeId: number;
  financialActivityCode: Api_TypeCode<number> | null;
  pretaxAmount: number | null;
  isGstRequired: boolean | null;
  taxAmount: number | null;
  totalAmount: number | null;
  isDisabled: boolean | null;
}
