import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_LeasePayment } from './LeasePayment';
import Api_TypeCode from './TypeCode';

export interface Api_LeaseTerm extends Api_ConcurrentVersion {
  id: number | null;
  leaseId: number;
  statusTypeCode: Api_TypeCode<string> | null;
  expiryDate: string | null;
  renewalDate: string | null;
  endDateHist: string | null;
  leasePmtFreqTypeCode: Api_TypeCode<string> | null;
  paymentAmount: number | null;
  gstAmount: number | null;
  paymentDueDate: string | null;
  paymentNote: string | null;
  isGstEligible: boolean | null;
  isTermExercised: boolean | null;
  startDate: string;
  effectiveDateHist: string | null;
  payments: Api_LeasePayment[];
}
