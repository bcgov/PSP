import { IOrganization } from './organization';
import { IRole } from './role';

export interface IUserDetails {
  id?: number;
  username?: string;
  key?: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  lastName?: string;
  position?: string | null;
  isDisabled?: boolean;
  note?: string;
  lastLogin?: string;
  emailVerified?: boolean;
  organizations?: IOrganization[];
  roles?: IRole[];
  createdOn?: string;
  rowVersion: number;
}
