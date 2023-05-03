import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Compensation extends Api_ConcurrentVersion {
  id: number;
  acquisitionFileId: number;
  isDraft: boolean | null;
  fiscalYear: string | null;
  agreementDateTime: string | null;
  expropriationNoticeServedDateTime: string | null;
  expropriationVestingDateTime: string | null;
  generationDatetTime: string | null;
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
