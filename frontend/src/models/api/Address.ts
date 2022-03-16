import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import Api_TypeCode from './TypeCode';

export interface Api_Country extends Api_ConcurrentVersion {
  countryId?: number;
  countryCode?: string;
  description?: string;
}

export interface Api_ProvinceState extends Api_ConcurrentVersion {
  provinceStateId?: number;
  provinceStateCode?: string;
  description?: string;
}

export interface Api_Address extends Api_ConcurrentVersion {
  id?: number;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  province: Api_ProvinceState;
  country?: Api_Country;
  countryOther?: string | null;
  postal?: string;
}

export interface Api_OrganizationAddress extends Api_ConcurrentVersion {
  id?: number;
  isDisabled?: boolean;
  organization?: Api_Organization;
  address?: Api_Address;
  addressUsageType?: Api_TypeCode<string>;
}

export interface Api_PersonAddress extends Api_ConcurrentVersion {
  id?: number;
  isDisabled?: boolean;
  person?: Api_Person;
  address?: Api_Address;
  addressUsageType?: Api_TypeCode<string>;
}
