import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';

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
  regions?: ApiGen_Base_CodeType<string>[];
  isDisabled?: boolean;
  lastLogin?: UtcIsoDateTime;
  appCreateTimestamp?: UtcIsoDateTime;
  rowVersion?: number;
  landline?: string;
  mobile?: string;
  hasValidClaims: boolean;
}
