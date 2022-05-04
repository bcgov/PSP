import { PropertyStatusTypes } from 'constants';
import { AreaUnitTypes } from 'constants/areaUnitTypes';
import { PropertyTypes } from 'constants/propertyTypes';
import { VolumetricParcelTypes } from 'constants/volumetricParcelTypes';
import { VolumeUnitTypes } from 'constants/volumeUnitTypes';
import { GeoJsonProperties } from 'geojson';
import { Api_Address } from 'models/api/Address';
import { Api_Property } from 'models/api/Property';
import Api_TypeCode from 'models/api/TypeCode';
import { ReactNode } from 'react';

export interface UpdatePropertyDetailsFormModel {
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

export function fromApi(base: Api_Property): UpdatePropertyDetailsFormModel {
  throw new Error('Not implemented yet');
}

export function toApi(formValues: UpdatePropertyDetailsFormModel): Api_Property {
  throw new Error('Not implemented yet');
}

export const defaultUpdateProperty: UpdatePropertyDetailsFormModel = {
  id: undefined,
  pid: '',
  pin: undefined,
  zoning: '',
  zoningPotential: '',
  municipalZoning: '',
  notes: '',
  name: '',
  description: '',
  isSensitive: false,
  isProvincialPublicHwy: false,
  latitude: undefined,
  longitude: undefined,
  landArea: 0,
  landLegalDescription: '',
  areaUnit: { id: AreaUnitTypes.Hectares },
  volumetricMeasurement: 0,
  volumetricUnit: { id: VolumeUnitTypes.CubicMeters },
  volumetricType: { id: VolumetricParcelTypes.Airspace },
  isVolumetricParcel: 'false',
  propertyType: { id: PropertyTypes.Titled },
  status: { id: PropertyStatusTypes.FeeSimple },
  anomalies: [],
  tenure: [],
  roadType: [],
  adjacentLand: [],
  dataSource: undefined,
  dataSourceEffectiveDate: undefined,
  address: undefined,
  motiRegion: undefined,
  highwaysDistrict: undefined,
  electoralDistrict: undefined,
  isALR: false,
  firstNations: undefined,
};
