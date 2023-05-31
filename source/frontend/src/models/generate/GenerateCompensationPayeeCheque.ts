import { Api_CompensationPayeeCheque } from 'models/api/Compensation';
import { formatMoney } from 'utils';

export class Api_GenerateCompensationPayeeCheque {
  pre_tax_amount: string;
  tax_amount: string;
  total_amount: string;
  gst_amount: string;

  constructor(cheque: Api_CompensationPayeeCheque) {
    this.pre_tax_amount = formatMoney(cheque.preTaxAmount) ?? '';
    this.tax_amount = formatMoney(cheque.taxAmount) ?? '';
    this.total_amount = formatMoney(cheque.totalAmount) ?? '';
    this.gst_amount = cheque.gstAmount ?? '';
  }
}
