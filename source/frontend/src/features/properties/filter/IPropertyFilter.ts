/**
 * Property filter options used by Formik.
 */

import { IAutocompletePrediction } from '@/interfaces';

import { DmsCoordinates } from './CoordinateSearch/models';

export interface IPropertyFilter {
  /** Select one of the search options [address, pid, location]. */
  searchBy: string;
  /** The page number. */
  page: string | undefined;
  /** The quantity to return in a single request for paging. */
  quantity: string | undefined;
  /** The pid property identifier. */
  pid: string;
  /** The pin property identifier. */
  pin: string;
  /** The historical file number property identifiers. */
  historical: string;
  /** The address of the property. */
  address: string;
  /** The legal description of the property. */
  legalDescription: string | null;
  /** The geocoder-returned latitude  */
  latitude: number | string | undefined;
  /** The geocoder-returned longitude  */
  longitude: number | string | undefined;
  /** The plan number of the property. */
  planNumber?: string;
  /** The property ownership types */
  ownership: string;
  /** The coordinates to search by lat/long in degrees, minutes and seconds (DMS) */
  coordinates: DmsCoordinates | null;
  /** The geographic name of the property */
  name: string;

  /** Survey Parcel District */
  district: string | null;
  /** Survey Parcel Section */
  section: string | null;
  /** Survey Parcel Township */
  township: string | null;
  /** Survey Parcel Range */
  range: string | null;

  /** Survey Parcel District Lot */
  districtLot: string | null;

  /** PIMS project */
  project: IAutocompletePrediction;
  /** The property tenure cleanup */
  tenureCleanup: string;
}

export const defaultPropertyFilter: IPropertyFilter = {
  searchBy: 'pid',
  pid: '',
  pin: '',
  address: '',
  planNumber: '',
  legalDescription: null,
  latitude: '',
  longitude: '',
  historical: '',
  coordinates: null,
  page: undefined,
  quantity: undefined,
  ownership: 'isCoreInventory,isPropertyOfInterest,isOtherInterest',
  name: '',
  district: null,
  section: null,
  township: null,
  range: null,
  districtLot: null,
  project: null,
  tenureCleanup: '',
};
