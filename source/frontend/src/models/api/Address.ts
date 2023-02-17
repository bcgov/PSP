import { Api_CodeType } from './CodeType';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import Api_TypeCode from './TypeCode';

export interface Api_Address extends Api_ConcurrentVersion {
  id?: number;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  provinceStateId?: number;
  province?: Api_CodeType;
  countryId?: number;
  country?: Api_CodeType;
  district?: Api_CodeType;
  region?: Api_CodeType;
  countryOther?: string;
  postal?: string;
  latitude?: number;
  longitude?: number;
  comment?: string;
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
