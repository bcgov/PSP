import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { formatMoney } from '@/utils';

export class Api_GenerateCompensationFinancial {
  pretax_amount: string;
  tax_amount: string;
  total_amount: string;
  financial_activity_code: string;
  financial_activity_description: string;

  constructor(compensation: ApiGen_Concepts_CompensationFinancial) {
    this.pretax_amount = formatMoney(compensation.pretaxAmount) ?? '';
    this.tax_amount = formatMoney(compensation.taxAmount) ?? '';
    this.total_amount = formatMoney(compensation.totalAmount) ?? '';
    this.financial_activity_code = compensation.financialActivityCode?.code?.toString() ?? '';
    this.financial_activity_description = compensation.financialActivityCode?.description ?? '';
  }
}
