import { IOrganization, IPerson } from '.';
export interface ITenant {
  person?: IPerson;
  organization?: IOrganization;
  name?: string;
}
