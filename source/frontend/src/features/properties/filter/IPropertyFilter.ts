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
  /** The pin or pid property identifiers. */
  pinOrPid: string;
  /** The address of the property. */
  address: string;
  latitude: number | string | undefined;
  longitude: number | string | undefined;
  /** The plan number of the property. */
  planNumber?: string;
}

export const defaultPropertyFilter: IPropertyFilter = {
  searchBy: 'pinOrPid',
  pinOrPid: '',
  address: '',
  planNumber: '',
  latitude: '',
  longitude: '',
  page: undefined,
  quantity: undefined,
};
