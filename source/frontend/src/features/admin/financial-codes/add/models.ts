import { Api_FinancialCode } from 'models/api/FinancialCode';

export class FinancialCodeForm {
  id?: number;
  type?: string = '';
  code?: string = '';
  description?: string = '';
  displayOrder?: number;
  effectiveDate?: string;
  expiryDate?: string;

  toApi(): Api_FinancialCode {
    return {
      id: this.id,
      type: this.type,
      code: this.code,
      description: this.description,
      displayOrder: this.displayOrder !== undefined ? Number(this.displayOrder) : undefined,
      effectiveDate: this.effectiveDate,
      expiryDate: this.expiryDate,
    };
  }

  static fromApi(model: Api_FinancialCode): FinancialCodeForm {
    const newForm = new FinancialCodeForm();
    newForm.id = model.id;
    newForm.type = model.type;
    newForm.code = model.code;
    newForm.description = model.description;
    newForm.displayOrder = model.displayOrder;
    newForm.effectiveDate = model.effectiveDate;
    newForm.expiryDate = model.expiryDate;

    return newForm;
  }
}
