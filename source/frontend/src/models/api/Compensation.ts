import { Api_ConcurrentVersion } from './ConcurrentVersion';

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
}

export interface Api_CompensationFinancial extends Api_ConcurrentVersion {
  id: number;
  compensationId: number;
  pretaxAmount: number | null;
  taxAmount: number | null;
  totalAmount: number | null;
  isDisabled: boolean | null;
}
