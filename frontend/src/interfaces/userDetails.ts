import { IOrganization } from './organization';
import { IRole } from './role';

export interface IUserDetails {
  id?: number;
  businessIdentifier?: string;
  keycloakUserId?: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  surname?: string;
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
