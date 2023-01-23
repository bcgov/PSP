import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Project extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  projectStatusTypeCode?: Api_TypeCode<string>;
  businessFunctionCode?: any; // TODO: Match new code types
  costTypeCode?: any; // TODO: Match new code types
  workActivityCode?: any; // TODO: Match new code types
  regionCode?: Api_TypeCode<number>;
  code?: number;
  description?: string;
  note?: string;
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
