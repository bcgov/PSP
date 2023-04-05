import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Agreement extends Api_ConcurrentVersion, Api_AuditFields {
  agreementId: number | null;
  acquisitionFileId: number | null;
  agreementType: Api_TypeCode<string> | null;
  agreementDate: string | null;
  agreementStatus: boolean | null;
  completionDate: string | null;
  terminationDate: string | null;
  commencementDate: string | null;
  depositAmount: number | null;
  noLaterThanDays: number | null;
  purchasePrice: number | null;
  legalSurveyPlanNum: string | null;
  offerDate: string | null;
  expiryDateTime: string | null;
  signedDate: string | null;
  inspectionDate: string | null;
}
