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
  lastName?: string;
  email?: string;
  username?: string;
  roles?: IRole[];
  organizations?: IOrganization[];
  isDisabled?: boolean;
  lastLogin?: string;
  createdOn?: string;
  rowVersion: number;
}
