import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_ContactCountry extends Api_ConcurrentVersion {
  countryId: number;
  countryCode: string;
  description: string;
}

export interface Api_ContactProvince extends Api_ConcurrentVersion {
  provinceStateId: number;
  provinceStateCode: string;
  description: string;
}

export interface Api_ContactAddress extends Api_ConcurrentVersion {
  id: number;
  rowVersion: number;
  addressType: Api_TypeCode<string>;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  province: Api_ContactProvince;
  country?: Api_ContactCountry;
  postal?: string;
}
