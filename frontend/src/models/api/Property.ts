import Api_TypeCode from 'interfaces/ITypeCode';
import { Moment } from 'moment';

import { Api_Address } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Coordinate {
  /**
   * @format double
   */
  x?: number;

  /**
   * @format double
   */
  y?: number;
}

export interface Api_Geometry {
  coordinate?: Api_Coordinate;
}

export interface Api_Property extends Api_ConcurrentVersion {
  id?: number;
  pid?: number;
  pin?: number | '';
  status?: string;
  dataSource?: string;
  dataSourceEffectiveDate?: Date | string | Moment;
  classification?: string;
  tenure?: Api_TypeCode<string>;
  name?: string;
  description?: string;
  address?: Api_Address;
  region?: Api_TypeCode<number>;
  district?: Api_TypeCode<string>;
  location?: Api_Geometry;
  planNumber?: string;

  latitude?: number;
  longitude?: number;

  landArea?: number;
  landLegalDescription?: string;
  encumbranceReason?: string;
  isSensitive?: boolean;
  isOwned?: boolean;
  isPropertyOfInterest?: boolean;
  isVisibleToOtherAgencies?: boolean;
  zoning?: string;
  zoningPotential?: string;

  appCreateTimestamp?: Date | string | Moment;
  updatedOn?: Date | string | Moment;
  updatedByEmail?: string;
  updatedByName?: string;
}
