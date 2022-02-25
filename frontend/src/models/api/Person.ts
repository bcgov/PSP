import { Api_ContactAddress } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_ContactMethod } from './ContactMethod';
import { Api_Organization } from './Organization';

export interface Api_Person extends Api_ConcurrentVersion {
  id: number;
  isDisabled: boolean;
  surname: string;
  firstName: string;
  preferredName?: string;
  organizations?: Api_Organization[];
  addresses?: Api_ContactAddress[];
  contactMethods?: Api_ContactMethod[];
  comment?: string;
}
