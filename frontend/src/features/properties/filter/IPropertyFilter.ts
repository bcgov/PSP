import { PropertyClassificationTypes, PropertyTypes } from 'constants/index';

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
  /** comma-separated list of organizations to filter by */
  organizations: string;
  /** The address of the property. */
  address: string;
  /** The location of the property. */
  municipality: string;
  /** The classification of the property. */
  classificationId?: PropertyClassificationTypes;
  /** Select on of the property types [Land, Building]. */
  propertyType?: PropertyTypes;
  /** The minimum lot size of the property. */
  minLotSize: string;
  /** The maximum lot size of the property. */
  maxLotSize: string;
  /** The name of desired target */
  name?: string;
}
