import { Api_Organization } from './Organization';
import { Api_Person } from './Person';

export interface Api_Contact {
  id: string;
  person?: Api_Person;
  organization?: Api_Organization;
}
