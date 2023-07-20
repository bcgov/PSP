import Api_TypeCode from '@/interfaces/ITypeCode';
import { Api_Contact } from '@/models/api/Contact';

import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_SecurityDeposit extends Api_ConcurrentVersion {
  id: number | null;
  leaseId: number | null;
  description: string;
  amountPaid: number;
  depositDate: string;
  depositType: Api_TypeCode<string>;
  otherTypeDescription: string | null;
  contactHolder: Api_Contact | null;
  depositReturns: Api_SecurityDepositReturn[];
}

export interface Api_SecurityDepositReturn extends Api_ConcurrentVersion {
  id: number | null;
  parentDepositId: number;
  terminationDate: string;
  claimsAgainst: number | null;
  returnAmount: number;
  interestPaid: number | null;
  returnDate: string;
  contactHolder: Api_Contact | null;
}
