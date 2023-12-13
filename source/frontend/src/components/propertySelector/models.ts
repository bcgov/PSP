import { Polygon } from 'geojson';

export interface IMapProperty {
  propertyId?: number;
  pid?: string;
  pin?: string;
  latitude?: number;
  longitude?: number;
  polygon?: Polygon;
  planNumber?: string;
  address?: string;
  region?: number;
  regionName?: string;
  district?: number;
  districtName?: string;
  name?: string;
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
  searchBy?: string;
  address?: string;
}
