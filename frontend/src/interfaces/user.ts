import { IOrganization } from './organization';
import { IRole } from './role';

export interface IUser {
  id: number;
  key: string;
  displayName?: string;
  position?: string;
  note?: string;
  firstName?: string;
  middleName?: string;
  surname?: string;
  email?: string;
  businessIdentifier?: string;
  roles?: IRole[];
  organizations?: IOrganization[];
  isDisabled?: boolean;
  lastLogin?: string;
  createdOn?: string;
  rowVersion: number;
}
