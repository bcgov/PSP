import { Api_Product, Api_Project, defaultProject } from 'models/api/Project';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { toTypeCode } from 'utils/formUtils';

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

  toApi(parentId?: number | null): Api_Product {
    return {
      id: this.id,
      parentProject: !!parentId
        ? {
            ...defaultProject,
            id: parentId,
          }
        : null,
      code: this.code,
      description: this.description,
      startDate: this.startDate,
      costEstimate: !!this.costEstimate ? Number(this.costEstimate) : null,
      costEstimateDate: this.costEstimateDate,
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
      products: this.products?.map(x => x.toApi(this.id)),
      rowVersion: this.rowVersion ?? null,
      businessFunctionCode: null,
      costTypeCode: null,
      workActivityCode: null,
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
    newForm.products = model.products?.map(x => ProductForm.fromApi(x)) || [];

    return newForm;
  }
}
