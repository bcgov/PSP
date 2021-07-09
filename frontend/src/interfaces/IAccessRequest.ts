import { AccessRequestStatus } from 'constants/index';

export interface IAccessRequest {
  id: number;
  userId: number;
  user: IUser;
  roles: IAccessRequestRole[];
  agencies: IAccessRequestAgency[];
  note?: string | null;
  status: AccessRequestStatus;
  position?: string;
  createdOn?: string;
  rowVersion?: number;
}

interface IUser {
  id?: number;
  key?: string;
  username?: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  lastName?: string;
  position?: string | null;
  isDisabled?: boolean;
  note?: string;
  createdOn?: string;
  rowVersion?: number;
}

export interface IAccessRequestAgency {
  id: number;
  code?: string;
  name?: string;
  description?: string;
  isDisabled?: boolean;
  sortOrder?: number;
  createdOn?: string;
}

export interface IAccessRequestRole {
  id: number;
  name?: string;
  description?: string;
  isDisabled?: boolean;
  sortOrder?: number;
  createdOn?: string;
  updatedByName?: string;
  updatedByEmail?: string;
  rowVersion?: number;
}
