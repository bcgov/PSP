import { DmsCoordinates } from '@/features/properties/filter/CoordinateSearch/models';

export interface ILayerSearchCriteria {
  pid?: string;
  pin?: string;
  planNumber?: string;
  legalDescription?: string;
  searchBy?: string;
  address?: string;
  historical?: string;
  coordinates?: DmsCoordinates | null;
}
