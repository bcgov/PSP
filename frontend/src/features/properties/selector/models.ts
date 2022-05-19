export interface IMapProperty {
  id?: string;
  pid?: string;
  pin?: string;
  latitude?: number;
  longitude?: number;
  planNumber?: string;
  address?: string;
  legalDescription?: string;
  district?: string;
}

export interface ILayerSearchCriteria {
  pid?: string;
  pin?: string;
  planNumber?: string;
  searchBy?: string;
}
