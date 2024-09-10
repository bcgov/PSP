import isNumber from 'lodash/isNumber';

import { SelectOption } from '@/components/common/form';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { booleanToString, stringToBoolean } from '@/utils/formUtils';

export class FinancialActivityFormModel {
  readonly _id: number | null = null;
  readonly _compensationRequisitionId: number;

  financialActivityCodeId: SelectOption | null = null;
  financialActivityCode: ApiGen_Concepts_FinancialCode | null = null;
  pretaxAmount = 0;
  isGstRequired = '';
  taxAmount = 0;
  totalAmount = 0;
  rowVersion: number | null = null;
  isDisabled = '';

  constructor(id: number | null = null, compensationRequisitionId: number) {
    this._id = id;
    this._compensationRequisitionId = compensationRequisitionId;
  }

  isEmpty(): boolean {
    return this.financialActivityCode === null && this.pretaxAmount === 0;
  }

  toApi(): ApiGen_Concepts_CompensationFinancial {
    return {
      id: this._id,
      compensationId: this._compensationRequisitionId,
      financialActivityCodeId:
        !!this.financialActivityCodeId?.value && isNumber(this.financialActivityCodeId?.value)
          ? Number(this.financialActivityCodeId?.value)
          : 0,
      financialActivityCode: null,
      pretaxAmount: this.pretaxAmount,
      isGstRequired: stringToBoolean(this.isGstRequired),
      taxAmount: this.taxAmount,
      totalAmount: this.totalAmount,
      isDisabled: stringToBoolean(this.isDisabled),
      h120CategoryId: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(
    model: ApiGen_Concepts_CompensationFinancial,
    financialActivityOptions: SelectOption[] = [],
  ): FinancialActivityFormModel {
    const newForm = new FinancialActivityFormModel(model.id, model.compensationId);
    newForm.pretaxAmount = model.pretaxAmount ?? 0;
    newForm.isGstRequired = booleanToString(model.isGstRequired);
    newForm.taxAmount = model.taxAmount ?? 0;
    newForm.totalAmount = model.totalAmount ?? 0;
    newForm.financialActivityCodeId =
      !!model.financialActivityCodeId && isNumber(model.financialActivityCodeId)
        ? financialActivityOptions.find(c => +c.value === model.financialActivityCodeId) ?? null
        : null;
    newForm.financialActivityCode = model.financialActivityCode;
    newForm.rowVersion = model.rowVersion ?? null;
    newForm.isDisabled = booleanToString(model.isDisabled);

    return newForm;
  }
}
