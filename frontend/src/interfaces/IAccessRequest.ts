import { AccessRequestStatus } from 'constants/index';
import { IOrganization, IRole, IUser } from 'interfaces';
import { IOrganization } from './organization';
import { IRole } from './role';

export interface IAccessRequest {
  id?: number;
  userId: number;
  user: IUser;
  note?: string | null;
  status: AccessRequestStatus;
  position?: string;
  organization?: IOrganization;
  organizationId?: number | '';
  role?: IRole;
  roleId?: number | '';
  createdOn?: string;
  rowVersion?: number;
}
