import { AccessRequestStatus } from 'constants/index';

import { IOrganization } from './organization';
import { IRole } from './role';

export interface IAccessRequest {
  id: number;
  userId: number;
  user: IUser;
  note?: string | null;
  status: AccessRequestStatus;
  position?: string;
  createdOn?: string;
  rowVersion?: number;
  organization?: IOrganization;
  organizationId?: number | '';
  role?: IRole;
  roleId?: number | '';
}

interface IUser {
  id?: number;
  key?: string;
  businessIdentifier?: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  surname?: string;
  position?: string | null;
  isDisabled?: boolean;
  note?: string;
  createdOn?: string;
  rowVersion?: number;
}

export interface IAccessRequestOrganization {
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
