/**
 * Property filter options used by Formik.
 */

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
  /** Survey Parcel Section */
  section: string;
  /** Survey Parcel Township */
  township: string;
  /** Survey Parcel Range */
  range: string;
  /** Survey Parcel District */
  district: string;
}

export const defaultPropertyFilter: IPropertyFilter = {
  searchBy: 'pid',
  pid: '',
  pin: '',
  address: '',
  planNumber: '',
  latitude: '',
  longitude: '',
  historical: '',
  coordinates: null,
  page: undefined,
  quantity: undefined,
  ownership: 'isCoreInventory,isPropertyOfInterest,isOtherInterest',
  name: '',
  section: '',
  township: '',
  range: '',
  district: '',
};
