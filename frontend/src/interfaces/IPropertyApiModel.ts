import { Api_Address } from 'models/api/Address';
import { Api_ConcurrentVersion } from 'models/api/ConcurrentVersion';
import Api_TypeCode from 'models/api/TypeCode';

import { ILease } from './ILease';

/**
 * Pairs with the current implementation of Pims.Api.Areas.Property.Models.Property.PropertyModel
 */
export interface IPropertyApiModel extends Api_ConcurrentVersion {
  id?: number;
  pid?: string;
  pin?: number;
  zoning?: string;
  zoningPotential?: string;
  municipalZoning?: string;
  notes?: string;

  name?: string;
  description?: string;
  isSensitive?: boolean;
  isProvincialPublicHwy?: boolean;
  pphStatusTypeCode?: string;
  isRwyBeltDomPatent?: boolean;

  latitude?: number;
  longitude?: number;

  landArea?: number;
  landLegalDescription?: string;
  areaUnit?: Api_TypeCode<string>;

  isVolumetricParcel?: boolean;
  volumetricMeasurement?: number;
  volumetricUnit?: Api_TypeCode<string>;
  volumetricType?: Api_TypeCode<string>;

  propertyType?: Api_TypeCode<string>;
  status?: Api_TypeCode<string>;

  regionType?: Api_TypeCode<number>;
  districtType?: Api_TypeCode<number>;

  // multi-selects

  leases?: ILease[];
  anomalies?: Api_TypeCode<string>[];
  tenure?: Api_TypeCode<string>[];
  roadType?: Api_TypeCode<string>[];
  adjacentLand?: Api_TypeCode<string>[];

  dataSource?: Api_TypeCode<string>;
  dataSourceEffectiveDate?: string;

  address?: Api_Address;
}
