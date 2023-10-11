import { Api_SecurityDeposit } from '@/models/api/SecurityDeposit';

import { formatMoney } from './../../../utils/numberFormatUtils';

export class Api_GenerateSecurityDeposit {
  security_amount: string;
  constructor(securityDeposit: Api_SecurityDeposit) {
    this.security_amount = formatMoney(securityDeposit?.amountPaid ?? 0);
  }
}
