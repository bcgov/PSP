import { AccessRequestStatus } from '@/constants/index';
import { IOrganization, IRole, IUser } from '@/interfaces';

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
  appCreateTimestamp?: string;
  rowVersion?: number;
}
