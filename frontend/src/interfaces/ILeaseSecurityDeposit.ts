import { Moment } from 'moment';

export interface ILeaseSecurityDeposit {
  id?: number;
  securityDepositHolderTypeId: string;
  securityDepositHolderType: string;
  securityDepositTypeId: string;
  securityDepositType: string;
  description: string;
  amountPaid: number;
  totalAmount: number;
  depositDate: Date | string | Moment;
  annualInterestRate: number;
}
