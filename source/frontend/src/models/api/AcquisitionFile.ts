import { Api_File } from 'models/api/File';
import Api_TypeCode from 'models/api/TypeCode';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Person } from './Person';
import { Api_Product, Api_Project } from './Project';
import { Api_PropertyFile } from './PropertyFile';

export interface Api_AcquisitionFile extends Api_ConcurrentVersion, Api_AuditFields, Api_File {
  id?: number;
  fileNo?: number;
  assignedDate?: string;
  deliveryDate?: string;
  // Code Tables
  acquisitionPhysFileStatusTypeCode?: Api_TypeCode<string>;
  acquisitionTypeCode?: Api_TypeCode<string>;
  // MOTI region
  regionCode?: Api_TypeCode<number>;
  acquisitionTeam?: Api_AcquisitionFilePerson[];

  project?: Api_Project;
  product?: Api_Product;
  fundingTypeCode?: Api_TypeCode<string>;
  fundingOther?: string;
}

export interface Api_AcquisitionFileProperty
  extends Api_ConcurrentVersion,
    Api_PropertyFile,
    Api_AuditFields {}

export interface Api_AcquisitionFilePerson extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  personId?: number;
  person?: Api_Person;
  personProfileTypeCode?: string;
  personProfileType?: Api_TypeCode<string>;
  isDisabled?: boolean;
}
