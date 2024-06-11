import { MultiPolygon, Polygon } from 'geojson';

import { AreaUnitTypes } from '@/constants';

export interface IMapProperty {
  propertyId?: number;
  pid?: string;
  pin?: string;
  latitude?: number;
  longitude?: number;
  polygon?: Polygon | MultiPolygon;
  planNumber?: string;
  address?: string;
  legalDescription?: string;
  region?: number;
  regionName?: string;
  district?: number;
  districtName?: string;
  name?: string;
  landArea?: number;
  areaUnit?: AreaUnitTypes;
}

export interface SearchResultProperty extends IMapProperty {
  id: string;
}

export interface PimsMapProperty extends IMapProperty {
  pimsId: string;
}

export interface ILayerSearchCriteria {
  pid?: string;
  pin?: string;
  planNumber?: string;
  legalDescription?: string;
  searchBy?: string;
  address?: string;
  historical?: string;
}
