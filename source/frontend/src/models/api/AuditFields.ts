import { UtcIsoDateTime } from './UtcIsoDateTime';

export interface Api_AuditFields {
  appCreateTimestamp?: UtcIsoDateTime;
  appLastUpdateTimestamp?: UtcIsoDateTime;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
  appLastUpdateUserGuid?: string;
  appCreateUserGuid?: string;
}
