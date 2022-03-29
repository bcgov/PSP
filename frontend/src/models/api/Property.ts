import { Api_Address } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Property extends Api_ConcurrentVersion {
  id?: number;
  pid?: string;
  pin?: number;
  zoning?: string;
  municipalZoning?: string;
  notes?: string;

  name?: string;
  description?: string;
  isSensitive: boolean;
  isProvincialPublicHwy?: boolean;

  latitude?: number;
  longitude?: number;

  landArea?: number;
  areaUnit?: Api_TypeCode<string>;

  isVolumetricParcel?: boolean;
  volumetricMeasurement?: number;
  volumetricUnit?: Api_TypeCode<string>;
  volumetricType?: Api_TypeCode<string>;

  propertyType: Api_TypeCode<string>;
  status?: Api_TypeCode<string>;

  // multi-selects
  anomalies?: Api_TypeCode<string>[];
  tenure: Api_TypeCode<string>[];
  roadType?: Api_TypeCode<string>[];
  adjacentLand?: Api_TypeCode<string>[];

  dataSource?: Api_TypeCode<string>;
  dataSourceEffectiveDate?: string;

  address: Api_Address;
}
