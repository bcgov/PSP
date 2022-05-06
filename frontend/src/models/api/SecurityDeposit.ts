import Api_TypeCode from 'interfaces/ITypeCode';
import { Api_Contact } from 'models/api/Contact';

import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_SecurityDeposit extends Api_ConcurrentVersion {
  id?: number;
  description: string;
  amountPaid: number;
  depositDate?: string;
  depositType: Api_TypeCode<string>;
  otherTypeDescription?: string;
  contactHolder?: Api_Contact;
  depositReturns: Api_SecurityDepositReturn[];
}

export interface Api_SecurityDepositReturn extends Api_ConcurrentVersion {
  id?: number;
  parentDepositId: number;
  terminationDate?: string;
  claimsAgainst?: number;
  returnAmount: number;
  interestPaid: number;
  returnDate?: string;
  contactHolder?: Api_Contact;
}
