import { Moment } from 'moment';

export interface ILeaseSecurityDepositReturn {
  id?: number;
  securityDepositTypeId: string;
  securityDepositType: string;
  terminationDate: Date | string | Moment;
  depositTotal: number;
  claimsAgainst?: number;
  returnAmount: number;
  returnDate: Date | string | Moment;
  chequeNumber: string;
  payeeName: string;
  payeeAddress: string;
  terminationNote: string;
}
