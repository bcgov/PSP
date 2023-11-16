import { Api_AcquisitionFile } from './AcquisitionFile';
import { Api_AuditFields } from './AuditFields';
import { Api_CodeType } from './CodeType';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { Api_FinancialCode } from './FinancialCode';
import Api_TypeCode from './TypeCode';

// LINK @backend/api/Models/Concepts/Project/ProjectModel.cs
export interface Api_Project extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  projectStatusTypeCode: Api_TypeCode<string> | null;
  businessFunctionCode: Api_FinancialCode | null;
  costTypeCode: Api_FinancialCode | null;
  workActivityCode: Api_FinancialCode | null;
  regionCode: Api_CodeType | null;
  code: string | null;
  description: string | null;
  note: string | null;
  projectProducts: Api_ProjectProduct[];
}

// LINK @backend/api/Models/Concepts/Product/ProductModel.cs
export interface Api_Product extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  projectProducts: Api_ProjectProduct[];
  acquisitionFiles: Api_AcquisitionFile[];
  code: string;
  description: string;
  startDate: string | null;
  costEstimate: number | null;
  costEstimateDate: string | null;
  objective: string;
  scope: string | null;
}

// LINK @backend/api/Models/Concepts/Project/ProjectProductModel.cs
export interface Api_ProjectProduct extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number;
  projectId: number;
  product: Api_Product | null;
  productId: number;
  project: Api_Project | null;
}
