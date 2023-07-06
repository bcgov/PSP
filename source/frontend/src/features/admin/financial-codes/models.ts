import moment from 'moment';

import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { stringToUndefined } from '@/utils/formUtils';

export class FinancialCodeForm {
  id?: number;
  rowVersion?: number;
  type?: string = '';
  code?: string = '';
  description?: string = '';
  displayOrder?: number;
  effectiveDate?: string = moment().format('YYYY-MM-DD');
  expiryDate?: string;

  toApi(): Api_FinancialCode {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      type: this.type,
      code: this.code,
      description: this.description,
      displayOrder: this.displayOrder !== undefined ? Number(this.displayOrder) : undefined,
      effectiveDate: this.effectiveDate,
      expiryDate: stringToUndefined(this.expiryDate),
    };
  }

  static fromApi(model: Api_FinancialCode): FinancialCodeForm {
    const newForm = new FinancialCodeForm();
    newForm.id = model.id;
    newForm.rowVersion = model.rowVersion;
    newForm.type = model.type;
    newForm.code = model.code;
    newForm.description = model.description;
    newForm.displayOrder = model.displayOrder;
    newForm.effectiveDate = model.effectiveDate ?? '';
    newForm.expiryDate = model.expiryDate ?? '';

    return newForm;
  }
}
