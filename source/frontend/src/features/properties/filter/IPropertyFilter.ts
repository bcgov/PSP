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
  /** The pin or pid property identifiers. */
  pinOrPid: string;
  /** The address of the property. */
  address: string;
}
