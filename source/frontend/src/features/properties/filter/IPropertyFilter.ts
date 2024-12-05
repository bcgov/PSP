/**
 * Property filter options used by Formik.
 */
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
  latitude: number | string | undefined;
  longitude: number | string | undefined;
  /** The plan number of the property. */
  planNumber?: string;
  /** The property ownership types */
  ownership: string;
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
  page: undefined,
  quantity: undefined,
  ownership: 'isCoreInventory,isPropertyOfInterest,isOtherInterest',
};
