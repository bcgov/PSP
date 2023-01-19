import { Api_Lease } from 'models/api/Lease';
import Api_TypeCode from 'models/api/TypeCode';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Property } from './Property';

export interface Api_PropertyLease extends Api_ConcurrentVersion {
  id?: number;
  property?: Api_Property;
  lease?: Api_Lease;
  propertyName?: string;
  leaseArea?: number;
  areaUnitType?: Api_TypeCode<string>;
  isDisabled?: boolean;
  displayOrder?: number;
}
