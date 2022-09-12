import { Api_File } from 'models/api/File';
import Api_TypeCode from 'models/api/TypeCode';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Property } from './Property';

export interface Api_AcquisitionFile extends Api_ConcurrentVersion, Api_AuditFields, Api_File {
  id?: number;
  ministryProjectNumber?: string;
  ministryProjectName?: string;
  assignedDate?: string;
  deliveryDate?: string;
  // Code Tables
  acquisitionPhysFileStatusTypeCode?: Api_TypeCode<string>;
  acquisitionTypeCode?: Api_TypeCode<string>;
  // MOTI region
  regionCode?: Api_TypeCode<number>;
  acquisitionProperties?: Api_AcquisitionFileProperty[];
}

export interface Api_AcquisitionFileProperty extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  isDisabled?: boolean;
  displayOrder?: number;
  propertyName?: string;
  property?: Api_Property;
  acquisitionFile?: Api_AcquisitionFile;
}
