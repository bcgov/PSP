import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';

import { formatMoney } from './../../../utils/numberFormatUtils';

export class Api_GenerateSecurityDeposit {
  security_amount: string;
  constructor(securityDeposit: ApiGen_Concepts_SecurityDeposit) {
    this.security_amount = formatMoney(securityDeposit?.amountPaid ?? 0);
  }
}
