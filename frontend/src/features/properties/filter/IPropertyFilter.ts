import { PropertyTypes } from 'constants/index';

/**
 * Property filter options used by Formik.
 */
export interface IPropertyFilter {
  /** Select one of the search options [address, pid, location]. */
  searchBy: string;
  /** The page number. */
  page?: string;
  /** The quantity to return in a single request for paging. */
  quantity?: string;
  /** The unique PID of the parcel. */
  pid: string;
  /** The unique PID of the parcel. */
  pin: string;
  /** The address of the property. */
  address: string;
  /** The location of the property. */
  location: string;
  /** Select on of the property types [Land, Building]. */
  propertyType?: PropertyTypes;
}
