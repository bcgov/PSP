import { Api_ContactAddress } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_ContactMethod } from './ContactMethod';
import { Api_Person } from './Person';

export interface Api_Organization extends Api_ConcurrentVersion {
  id: number;
  isDisabled: boolean;
  name: string;
  alias: string;
  incorporationNumber: string;
  persons?: Api_Person[];
  addresses?: Api_ContactAddress[];
  contactMethods?: Api_ContactMethod[];
  comment: string;
}
