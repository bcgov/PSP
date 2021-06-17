import { AccessRequestStatus } from 'constants/accessStatus';

export interface IAccessRequest {
  id: number;
  userId: string;
  user: IUser;
  agencies: any[];
  roles: any[];
  note?: string | null;
  status: AccessRequestStatus;
  rowVersion?: number;
  createdOn?: string;
  position?: string;
}

interface IUser {
  id?: string;
  username?: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  lastName?: string;
  position?: string | null;
  isDisabled?: boolean;
  createdOn?: string;
  rowVersion?: number;
  note?: string;
}
