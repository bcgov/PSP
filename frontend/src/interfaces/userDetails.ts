import { IAgency } from './agency';
import { IRole } from './role';

export interface IUserDetails {
  id: number;
  username?: string;
  key: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  lastName?: string;
  position?: string | null;
  isDisabled?: boolean;
  note?: string;
  lastLogin?: string;
  emailVerified?: boolean;
  agencies?: IAgency[];
  roles?: IRole[];
  createdOn?: string;
  rowVersion: number;
}
