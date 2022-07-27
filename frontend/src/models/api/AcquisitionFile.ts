import Api_TypeCode from 'models/api/TypeCode';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_AcquisitionFile extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  name?: string;
  assignedDate?: string;
  deliveryDate?: string;
  // Code Tables
  acquisitionFileStatusTypeCode?: Api_TypeCode<string>;
  acquisitionPhysFileStatusTypeCode?: Api_TypeCode<string>;
  acquisitionTypeCode?: Api_TypeCode<string>;
  // MOTI region
  regionCode?: Api_TypeCode<number>;
}
