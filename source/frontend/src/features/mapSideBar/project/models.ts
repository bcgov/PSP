import { Api_Product, Api_Project, Api_ProjectProduct } from '@/models/api/Project';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { fromTypeCode, stringToUndefined, toTypeCode } from '@/utils/formUtils';

export class ProductForm {
  id: number | null = null;

  code: string = '';
  description: string = '';
  startDate: string | '' = '';
  costEstimate: string = '';
  costEstimateDate: string | '' = '';
  objective: string | '' = '';
  scope: string | '' = '';
  rowVersion: number | null = null;

  toApi(): Api_Product {
    return {
      id: this.id,
      projectProducts: [],
      code: this.code,
      description: this.description,
      startDate: stringToUndefined(this.startDate),
      costEstimate: !!this.costEstimate ? Number(this.costEstimate) : null,
      costEstimateDate: stringToUndefined(this.costEstimateDate),
      objective: this.objective,
      scope: this.scope,
      rowVersion: this.rowVersion,
      acquisitionFiles: [],
    };
  }

  static fromApi(model: Api_Product): ProductForm {
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
  businessFunctionCode: NumberFieldValue | '' = '';
  costTypeCode: NumberFieldValue | '' = '';
  workActivityCode: NumberFieldValue | '' = '';
  region: NumberFieldValue | '' = '';
  summary: string | '' = '';
  rowVersion: number | null = null;
  products: ProductForm[] = [];

  toApi(): Api_Project {
    return {
      id: this.id,
      code: this.projectNumber ?? null,
      description: this.projectName ?? null,
      projectStatusTypeCode: toTypeCode<string>(this.projectStatusType) ?? null,
      regionCode: this.region ? toTypeCode<number>(+this.region) ?? null : null,
      note: this.summary ?? null,
      projectProducts: this.products?.map<Api_ProjectProduct>(x => {
        return {
          id: 0,
          projectId: 0,
          product: x.toApi(),
          productId: 0,
          project: null,
          rowVersion: 0,
        };
      }),
      rowVersion: this.rowVersion ?? null,
      businessFunctionCode: this.businessFunctionCode
        ? toTypeCode<number>(+this.businessFunctionCode) ?? null
        : null,
      costTypeCode: this.costTypeCode ? toTypeCode<number>(+this.costTypeCode) ?? null : null,
      workActivityCode: this.workActivityCode
        ? toTypeCode<number>(+this.workActivityCode) ?? null
        : null,
    };
  }

  static fromApi(model: Api_Project): ProjectForm {
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
        .map(x => x.product)
        ?.filter((x): x is Api_Product => x !== null)
        .map(x => ProductForm.fromApi(x)) || [];
    newForm.businessFunctionCode = model.businessFunctionCode?.id
      ? fromTypeCode<number>(model.businessFunctionCode) ?? ''
      : '';
    newForm.costTypeCode = model.costTypeCode?.id
      ? fromTypeCode<number>(model.costTypeCode) ?? ''
      : '';
    newForm.workActivityCode = model.workActivityCode?.id
      ? fromTypeCode<number>(model.workActivityCode) ?? ''
      : '';

    return newForm;
  }
}
