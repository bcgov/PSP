import { Api_AcquisitionFile } from './AcquisitionFile';
import { Api_AuditFields } from './AuditFields';
import { Api_CodeType } from './CodeType';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { Api_FinancialCode } from './FinancialCode';
import Api_TypeCode from './TypeCode';

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
  products: Api_Product[];
}

export interface Api_Product extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id?: number | null;
  parentProject: Api_Project | null;
  parentProjectId: number | null;
  code: string | null;
  description: string | null;
  startDate: string | null;
  costEstimate: number | null;
  costEstimateDate: string | null;
  objective: string | null;
  scope: string | null;
  acquisitionFiles: Api_AcquisitionFile[];
}
