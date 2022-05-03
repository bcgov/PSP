import { GeoJsonProperties } from 'geojson';
import { Api_Address } from 'models/api/Address';
import { Api_Property } from 'models/api/Property';
import Api_TypeCode from 'models/api/TypeCode';
import { ReactNode } from 'react';

export interface UpdatePropertyDetailsForm {
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

  latitude?: number;
  longitude?: number;

  landArea?: number;
  landLegalDescription?: string;
  areaUnit?: Api_TypeCode<string>;

  volumetricMeasurement?: number;
  volumetricUnit?: Api_TypeCode<string>;
  volumetricType?: Api_TypeCode<string>;

  propertyType?: Api_TypeCode<string>;
  status?: Api_TypeCode<string>;

  // multi-selects

  anomalies?: Api_TypeCode<string>[];
  tenure?: Api_TypeCode<string>[];
  roadType?: Api_TypeCode<string>[];
  adjacentLand?: Api_TypeCode<string>[];

  dataSource?: Api_TypeCode<string>;
  dataSourceEffectiveDate?: string;

  address?: Api_Address;

  motiRegion?: GeoJsonProperties;
  highwaysDistrict?: GeoJsonProperties;
  electoralDistrict?: GeoJsonProperties;
  isALR?: boolean;
  firstNations?: {
    bandName?: string;
    reserveName?: string;
  };
  isVolumetricParcel: string; // radio buttons only support string values, not booleans
  landMeasurementTable?: Array<{ value: number; unit: string }>;
  volumetricMeasurementTable?: Array<{ value: number; unit: ReactNode }>;
}

export function fromApi(base: Api_Property): UpdatePropertyDetailsForm {
  throw new Error('Not implemented yet');
}

export function toApi(formValues: UpdatePropertyDetailsForm): Api_Property {
  throw new Error('Not implemented yet');
}
