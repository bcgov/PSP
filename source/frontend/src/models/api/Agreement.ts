import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { DateOnly } from './DateOnly';
import Api_TypeCode from './TypeCode';
import { UtcIsoDateTime } from './UtcIsoDateTime';

export interface Api_Agreement extends Api_ConcurrentVersion, Api_AuditFields {
  agreementId: number;
  acquisitionFileId: number;
  agreementType: Api_TypeCode<string>;
  agreementDate: DateOnly | null;
  isDraft: boolean | null;
  completionDate: DateOnly | null;
  terminationDate: DateOnly | null;
  commencementDate: DateOnly | null;
  possessionDate: DateOnly | null;
  depositAmount: number | null;
  noLaterThanDays: number | null;
  purchasePrice: number | null;
  legalSurveyPlanNum: string | null;
  offerDate: DateOnly | null;
  expiryDateTime: UtcIsoDateTime | null;
  signedDate: DateOnly | null;
  inspectionDate: DateOnly | null;
}

export enum AgreementTypes {
  H0074 = 'H0074',
  H179A = 'H179A',
  H179P = 'H179P',
  H179T = 'H179T',
}
