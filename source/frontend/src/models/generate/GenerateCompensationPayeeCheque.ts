import { Api_PayeeCheque } from 'models/api/PayeeCheque';
import { formatMoney } from 'utils';

export class Api_GenerateCompensationPayeeCheque {
  pre_tax_amount: string;
  tax_amount: string;
  total_amount: string;
  gst_number: string;

  constructor(cheque: Api_PayeeCheque) {
    this.pre_tax_amount = formatMoney(cheque.pretaxAmount) ?? '';
    this.tax_amount = formatMoney(cheque.taxAmount) ?? '';
    this.total_amount = formatMoney(cheque.totalAmount) ?? '';
    this.gst_number = cheque.gstNumber ?? '';
  }
}
