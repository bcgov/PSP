import { isNumber } from 'lodash';

import { SelectOption } from '@/components/common/form';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { ApiGen_Concepts_ProjectProduct } from '@/models/api/generated/ApiGen_Concepts_ProjectProduct';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { toFinancialCode, toTypeCodeNullable } from '@/utils/formUtils';
import { exists, isValidIsoDateTime } from '@/utils/utils';

export class ProductForm {
  id: number | null = null;

  code = '';
  description = '';
  startDate: string | '' = '';
  costEstimate = '';
  costEstimateDate: string | '' = '';
  objective: string | '' = '';
  scope: string | '' = '';
  rowVersion: number | null = null;

  toApi(): ApiGen_Concepts_Product {
    return {
      id: this.id,
      projectProducts: [],
      code: this.code,
      description: this.description,
      startDate: isValidIsoDateTime(this.startDate) ? this.startDate : null,
      costEstimate: this.costEstimate ? Number(this.costEstimate) : null,
      costEstimateDate: isValidIsoDateTime(this.costEstimateDate) ? this.costEstimateDate : null,
      objective: this.objective,
      scope: this.scope,
      acquisitionFiles: [],
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_Product): ProductForm {
    const newForm = new ProductForm();
    newForm.id = model.id ?? null;
    newForm.code = model.code || '';
    newForm.description = model.description || '';
    newForm.startDate = model.startDate || '';
    newForm.costEstimate = model.costEstimate?.toString() || '';
    newForm.costEstimateDate = model.costEstimateDate || '';
    newForm.objective = model.objective || '';
    newForm.scope = model.scope || '';
    newForm.rowVersion = model.rowVersion ?? null;

    return newForm;
  }
}

export class ProjectForm {
  id: number | null = null;
  projectName: string | '' = '';
  projectNumber: string | '' = '';
  projectStatusType: string | '' = '';
  businessFunctionCode: SelectOption | null = null;
  costTypeCode: SelectOption | null = null;
  workActivityCode: SelectOption | null = null;
  region: NumberFieldValue | '' = '';
  summary: string | '' = '';
  rowVersion: number | null = null;
  products: ProductForm[] = [];

  toApi(): ApiGen_Concepts_Project {
    return {
      id: this.id ?? 0,
      code: this.projectNumber ?? null,
      description: this.projectName ?? null,
      projectStatusTypeCode: toTypeCodeNullable<string>(this.projectStatusType) ?? null,
      regionCode: this.region ? toTypeCodeNullable<number>(+this.region) ?? null : null,
      note: this.summary ?? null,
      projectProducts: this.products?.map<ApiGen_Concepts_ProjectProduct>(x => {
        return {
          id: 0,
          projectId: 0,
          product: x.toApi(),
          productId: 0,
          project: null,
          ...getEmptyBaseAudit(0),
        };
      }),
      businessFunctionCode:
        !!this.businessFunctionCode?.value && isNumber(this.businessFunctionCode.value)
          ? toFinancialCode(
              +this.businessFunctionCode.value,
              ApiGen_Concepts_FinancialCodeTypes.BusinessFunction,
            ) ?? null
          : null,
      costTypeCode:
        !!this.costTypeCode?.value && isNumber(this.costTypeCode.value)
          ? toFinancialCode(
              +this.costTypeCode.value,
              ApiGen_Concepts_FinancialCodeTypes.CostType,
            ) ?? null
          : null,
      workActivityCode:
        !!this.workActivityCode?.value && isNumber(this.workActivityCode.value)
          ? toFinancialCode(
              +this.workActivityCode.value,
              ApiGen_Concepts_FinancialCodeTypes.WorkActivity,
            ) ?? null
          : null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(
    model: ApiGen_Concepts_Project,
    businessFunctionOptions: SelectOption[] = [],
    costTypeOptions: SelectOption[] = [],
    workActivityOptions: SelectOption[] = [],
  ): ProjectForm {
    const newForm = new ProjectForm();
    newForm.id = model.id ?? null;
    newForm.projectName = model.description ?? '';
    newForm.projectNumber = model.code ?? '';
    newForm.projectStatusType = model.projectStatusTypeCode?.id ?? '';
    newForm.region = model.regionCode?.id ? +model.regionCode?.id ?? '' : '';
    newForm.summary = model.note ?? '';
    newForm.rowVersion = model.rowVersion ?? null;
    newForm.products =
      model.projectProducts
        ?.map(x => x.product)
        ?.filter(exists)
        .map(x => ProductForm.fromApi(x)) || [];
    newForm.businessFunctionCode =
      !!model.businessFunctionCode?.id && isNumber(model.businessFunctionCode?.id)
        ? businessFunctionOptions.find(c => +c.value === model.businessFunctionCode?.id) ?? null
        : null;
    newForm.costTypeCode =
      !!model.costTypeCode?.id && isNumber(model.costTypeCode?.id)
        ? costTypeOptions.find(c => +c.value === model.costTypeCode?.id) ?? null
        : null;
    newForm.workActivityCode =
      !!model.workActivityCode?.id && isNumber(model.workActivityCode?.id)
        ? workActivityOptions.find(c => +c.value === model.workActivityCode?.id) ?? null
        : null;

    return newForm;
  }
}
