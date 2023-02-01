import { Api_AuditFields } from './AuditFields';
import { Api_CodeType } from './CodeType';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Project extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  projectStatusTypeCode?: Api_TypeCode<string>;
  businessFunctionCode?: Api_CodeType;
  costTypeCode?: Api_CodeType;
  workActivityCode?: Api_CodeType;
  regionCode?: Api_TypeCode<number>;
  code?: string;
  description?: string;
  note?: string;
  products?: Api_Product[];
}

export interface Api_Product extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  parentProject?: Api_Project;
  code?: string;
  description?: string;
  startDate?: string;
  costEstimate?: number;
  costEstimateDate?: string;
  objective?: string;
  scope?: string;
}
