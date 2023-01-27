import { Api_PersonAddress } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_ContactMethod } from './ContactMethod';
import { Api_Organization } from './Organization';

export interface Api_Person extends Api_ConcurrentVersion {
  id?: number;
  isDisabled?: boolean;
  surname?: string;
  firstName?: string;
  middleNames?: string;
  preferredName?: string;
  personOrganizations?: Api_PersonOrganization[];
  personAddresses?: Api_PersonAddress[];
  contactMethods?: Api_ContactMethod[];
  comment?: string;
}

export interface Api_PersonOrganization extends Api_ConcurrentVersion {
  personId?: number;
  organization?: Api_Organization;
  isDisabled?: boolean;
}
