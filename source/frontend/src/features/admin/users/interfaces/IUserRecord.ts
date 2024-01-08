import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';

export interface IUserRecord {
  id?: number;
  keycloakUserId?: string;
  email?: string;
  businessIdentifierValue?: string;
  firstName?: string;
  surname?: string;
  isDisabled?: boolean;
  regions?: string;
  roles?: string;
  position?: string;
  lastLogin?: string;
  appCreateTimestamp?: UtcIsoDateTime;
  rowVersion?: number;
}
