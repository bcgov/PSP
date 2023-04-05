import { Api_Product, Api_Project } from 'models/api/Project';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { toTypeCode } from 'utils/formUtils';

export class ProductForm {
  id?: number;
  code: string = '';
  description: string = '';
  startDate?: string;
  costEstimate: string = '';
  costEstimateDate?: string;
  objective?: string;
  scope?: string;
  rowVersion?: number;

  toApi(parentId?: number): Api_Product {
    return {
      id: this.id,
      parentProject: parentId !== undefined ? { id: parentId } : undefined,
      code: this.code,
      description: this.description,
      startDate: this.startDate,
      costEstimate: this.costEstimate !== undefined ? Number(this.costEstimate) : undefined,
      costEstimateDate: this.costEstimateDate,
      objective: this.objective,
      scope: this.scope,
      rowVersion: this.rowVersion,
    };
  }

  static fromApi(model: Api_Product): ProductForm {
    const newForm = new ProductForm();
    newForm.id = model.id;
    newForm.code = model.code || '';
    newForm.description = model.description || '';
    newForm.startDate = model.startDate;
    newForm.costEstimate = model.costEstimate?.toString() || '';
    newForm.costEstimateDate = model.costEstimateDate;
    newForm.objective = model.objective;
    newForm.scope = model.scope;
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }
}

export class ProjectForm {
  id?: number;
  projectName?: string;
  projectNumber?: string;
  projectStatusType?: string;
  region?: NumberFieldValue;
  summary?: string;
  rowVersion?: number;
  products: ProductForm[] = [];

  constructor() {
    this.projectStatusType = 'AC';
  }

  toApi(): Api_Project {
    return {
      id: this.id,
      code: this.projectNumber,
      description: this.projectName,
      projectStatusTypeCode: toTypeCode<string>(this.projectStatusType),
      regionCode: this.region ? toTypeCode<number>(+this.region) : undefined,
      note: this.summary,
      products: this.products?.map(x => x.toApi(this.id)),
      rowVersion: this.rowVersion,
    };
  }

  static fromApi(model: Api_Project): ProjectForm {
    const newForm = new ProjectForm();
    newForm.id = model.id;
    newForm.projectName = model.description;
    newForm.projectNumber = model.code;
    newForm.projectStatusType = model.projectStatusTypeCode?.id;
    newForm.region = model.regionCode?.id ? +model.regionCode?.id : undefined;
    newForm.summary = model.note;
    newForm.rowVersion = model.rowVersion;
    newForm.products = model.products?.map(x => ProductForm.fromApi(x)) || [];

    return newForm;
  }
}
