import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { Api_PayeeCheque } from './PayeeCheque';

export interface Api_Payee extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  compensationRequisitionId: number | null;
  acquisitionOwnerId: number | null;
  interestHolderId: number | null;
  ownerRepresentativeId: number | null;
  ownerSolicitorId: number | null;
  acquisitionFilePersonId: number | null;
  cheques: Api_PayeeCheque[] | null;
}
