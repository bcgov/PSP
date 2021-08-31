/**
 * An address represents a location of a property or person.
 */
export interface IAddress {
  id?: number;
  addressTypeId: string;
  addressType?: string;
  regionId?: number;
  region?: string;
  districtId?: number;
  district?: string;
  provinceId: number;
  province?: string;
  countryId?: number;
  country?: string;
  streetAddress1: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality: string;
  postal: string;
  latitude?: number;
  longitude?: number;
  rowVersion?: number;
}
