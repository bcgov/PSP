import { Api_AcquisitionFile } from './AcquisitionFile';
import { Api_AuditFields } from './AuditFields';
import { Api_CodeType } from './CodeType';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Project extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  projectStatusTypeCode: Api_TypeCode<string> | null;
  businessFunctionCode: Api_CodeType | null;
  costTypeCode: Api_CodeType | null;
  workActivityCode: Api_CodeType | null;
  regionCode: Api_CodeType | null;
  code: string | null;
  description: string | null;
  note: string | null;
  products: Api_Product[];
}

export const defaultProject: Api_Project = {
  id: null,
  projectStatusTypeCode: null,
  businessFunctionCode: null,
  workActivityCode: null,
  code: null,
  costTypeCode: null,
  regionCode: null,
  description: null,
  note: null,
  products: [],
  rowVersion: null,
};

export interface Api_Product extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id?: number | null;
  parentProject: Api_Project | null;
  code: string | null;
  description: string | null;
  startDate: string | null;
  costEstimate: number | null;
  costEstimateDate: string | null;
  objective: string | null;
  scope: string | null;
  acquisitionFiles: Api_AcquisitionFile[];
}

export const defaultProduct: Api_Product = {
  parentProject: null,
  code: null,
  description: null,
  startDate: null,
  costEstimate: null,
  costEstimateDate: null,
  objective: null,
  scope: null,
  acquisitionFiles: [],
  rowVersion: null,
};
