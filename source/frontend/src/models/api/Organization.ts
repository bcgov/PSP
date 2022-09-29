import { Api_OrganizationAddress } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_ContactMethod } from './ContactMethod';
import { Api_Person } from './Person';

export interface Api_Organization extends Api_ConcurrentVersion {
  id?: number;
  isDisabled?: boolean;
  name?: string;
  alias?: string;
  incorporationNumber?: string;
  organizationPersons?: Api_OrganizationPerson[];
  organizationAddresses?: Api_OrganizationAddress[];
  contactMethods?: Api_ContactMethod[];
  comment?: string;
}

export interface Api_OrganizationPerson extends Api_ConcurrentVersion {
  person?: Api_Person;
  personId?: number;
  organizationId?: number;
  isDisabled?: boolean;
}
