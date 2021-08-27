import { AccessRequestStatus } from 'constants/index';
import { IOrganization, IRole, IUser } from 'interfaces';
export interface IAccessRequest {
  id?: number;
  userId: number;
  user: IUser;
  note?: string | null;
  status: AccessRequestStatus;
  position?: string;
  organizationId?: number | '';
  roleId?: number | '';
  createdOn?: string;
  rowVersion?: number;
}
