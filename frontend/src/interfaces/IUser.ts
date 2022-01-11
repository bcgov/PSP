import { Moment } from 'moment';

import { IOrganization } from './IOrganization';
import { IRole } from './IRole';

export interface IUser {
  id?: number;
  businessIdentifierValue?: string;
  businessIdentifier?: string;
  keycloakUserId?: string;
  email?: string;
  displayName?: string;
  position?: string;
  note?: string;
  firstName?: string;
  middleName?: string;
  surname?: string;
  roles?: IRole[];
  organizations?: IOrganization[];
  isDisabled?: boolean;
  lastLogin?: Date | string | Moment;
  appCreateTimestamp?: Date | string | Moment;
  rowVersion?: number;
  landline?: string;
  mobile?: string;
}
