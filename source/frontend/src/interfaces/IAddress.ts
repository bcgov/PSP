import { NumberFieldValue } from '@/typings/NumberFieldValue';

/**
 * An address represents a location of a property or person.
 */
export interface IAddress {
  id?: number;
  addressType?: string;
  regionId?: number;
  region?: string;
  districtId?: number;
  district?: string;
  provinceId: number;
  province?: string;
  provinceCode?: string;
  countryId?: number;
  country?: string;
  streetAddress1: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  postal?: string;
  latitude?: number;
  longitude?: number;
  rowVersion?: number;
}

export interface IFormAddress
  extends ExtendOverride<
    IAddress,
    {
      regionId?: NumberFieldValue;
      districtId?: NumberFieldValue;
      provinceId: NumberFieldValue;
      countryId?: NumberFieldValue;
      latitude?: NumberFieldValue;
      longitude?: NumberFieldValue;
      rowVersion?: NumberFieldValue;
    }
  > {}

export const defaultAddress: IFormAddress = {
  addressType: '',
  municipality: '',
  postal: '',
  provinceId: '',
  streetAddress1: '',
  country: '',
  countryId: '',
  district: '',
  districtId: '',
  latitude: '',
  longitude: '',
  province: '',
  region: '',
  regionId: '',
  streetAddress2: '',
  streetAddress3: '',
};
