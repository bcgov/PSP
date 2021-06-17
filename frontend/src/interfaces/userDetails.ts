import { IAgency } from './agency';
import { IRole } from './role';
export interface IUserDetails {
  id?: number;
  key?: string;
  username?: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  lastName?: string;
  position?: string | null;
  isDisabled?: boolean;
  agencies?: IAgency[];
  roles?: IRole[];
  createdOn?: string;
  rowVersion?: number;
  note?: string;
  lastLogin?: string;
  emailVerified?: boolean;
}
