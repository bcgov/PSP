import moment from 'moment';

import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isValidIsoDateTime } from '@/utils/utils';

export class FinancialCodeForm {
  id?: number;
  rowVersion?: number;
  type?: string = '';
  code?: string = '';
  description?: string = '';
  displayOrder?: number;
  effectiveDate?: string = moment().format('YYYY-MM-DD');
  expiryDate?: string;

  toApi(): ApiGen_Concepts_FinancialCode {
    return {
      id: this.id ?? 0,
      type: exists(this.type)
        ? (this.type as ApiGen_Concepts_FinancialCodeTypes)
        : ApiGen_Concepts_FinancialCodeTypes.BusinessFunction,
      code: this.code ?? null,
      description: this.description ?? null,
      displayOrder: this.displayOrder !== undefined ? Number(this.displayOrder) : null,
      effectiveDate: isValidIsoDateTime(this.effectiveDate) ? this.effectiveDate : EpochIsoDateTime,
      expiryDate: isValidIsoDateTime(this.expiryDate) ? this.expiryDate : null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_FinancialCode): FinancialCodeForm {
    const newForm = new FinancialCodeForm();
    newForm.id = model.id;
    newForm.rowVersion = model.rowVersion ?? undefined;
    newForm.type = model.type;
    newForm.code = model.code ?? undefined;
    newForm.description = model.description ?? undefined;
    newForm.displayOrder = model.displayOrder ?? undefined;
    newForm.effectiveDate = isValidIsoDateTime(model.effectiveDate) ? model.effectiveDate : '';
    newForm.expiryDate = isValidIsoDateTime(model.expiryDate) ? model.expiryDate : '';

    return newForm;
  }
}
