import { Api_AcquisitionFileOwner } from './AcquisitionFile';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

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
  responsibilityCode: Api_TypeCode<string> | null;
  chartOfAccountsCode: Api_TypeCode<string> | null;
  yearlyFinancialCode: Api_TypeCode<string> | null;
}

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
  acquisitionPayeeId: number;
  compensationRequisitionId: number;
  acquisitionOwnerId: number | null;
  interestHolderId: number | null;
  owner: Api_AcquisitionFileOwner;
  payeeCheques: Api_CompensationPayeeCheque[];
}

export interface Api_CompensationPayeeCheque extends Api_ConcurrentVersion {
  id?: number;
  preTaxAmount?: number;
  taxAmount?: number;
  totalAmount?: number;
  gstAmount?: string;
}
