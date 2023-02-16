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
    };
  }
}

export class ProjectForm {
  id?: number;
  projectName?: string;
  projectNumber?: string;
  projectStatusType?: string;
  region?: NumberFieldValue;
  summary?: string;
  products: ProductForm[] = [];

  toApi(): Api_Project {
    return {
      id: this.id,
      code: this.projectNumber,
      description: this.projectName,
      projectStatusTypeCode: toTypeCode<string>(this.projectStatusType),
      regionCode: this.region ? toTypeCode<number>(+this.region) : undefined,
      note: this.summary,
      products: this.products?.map(x => x.toApi(this.id)),
    };
  }
}
