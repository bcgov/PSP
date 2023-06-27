import { Api_Lease } from '@/models/api/Lease';
import Api_TypeCode from '@/models/api/TypeCode';

import { Api_Property } from './Property';
import { Api_PropertyFile } from './PropertyFile';

export interface Api_PropertyLease extends Api_PropertyFile {
  id?: number;
  property?: Api_Property;
  lease: Api_Lease | null;
  leaseId: number | null;
  propertyName?: string;
  leaseArea: number | null;
  areaUnitType: Api_TypeCode<string> | null;
}
