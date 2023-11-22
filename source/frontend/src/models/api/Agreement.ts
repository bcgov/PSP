import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Agreement extends Api_ConcurrentVersion, Api_AuditFields {
  agreementId: number;
  acquisitionFileId: number;
  agreementType: Api_TypeCode<string>;
  agreementStatusType: Api_TypeCode<string>;
  agreementDate: string | null;
  completionDate: string | null;
  terminationDate: string | null;
  commencementDate: string | null;
  possessionDate: string | null;
  depositAmount: number | null;
  noLaterThanDays: number | null;
  purchasePrice: number | null;
  legalSurveyPlanNum: string | null;
  offerDate: string | null;
  expiryDateTime: string | null;
  signedDate: string | null;
  inspectionDate: string | null;
  cancellationNote: string | null;
}

export enum AgreementTypes {
  H0074 = 'H0074',
  H179A = 'H179A',
  H179P = 'H179P',
  H179T = 'H179T',
}

export enum AgreementStatusTypes {
  DRAFT = 'DRAFT',
  FINAL = 'FINAL',
  CANCELLED = 'CANCELLED',
}
