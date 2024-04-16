import { formatMoney } from '@/utils/numberFormatUtils';

import { ApiGen_Concepts_ExpropriationPaymentItem } from '../api/generated/ApiGen_Concepts_ExpropriationPaymentItem';

export class Api_GenerateExpropriationPaymentItem {
  description: string;
  pretaxAmount: string;
  isGstRequired: boolean;
  taxAmount: string;
  totalAmount: string;

  constructor(item: ApiGen_Concepts_ExpropriationPaymentItem) {
    this.description = item.paymentItemType?.description ?? '';
    this.pretaxAmount = formatMoney(item.pretaxAmount ?? 0);
    this.isGstRequired = item.isGstRequired ?? false;
    this.taxAmount = formatMoney(item.taxAmount ?? 0);
    this.totalAmount = formatMoney(item.totalAmount ?? 0);
  }
}
