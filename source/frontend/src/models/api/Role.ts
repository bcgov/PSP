import { Api_ConcurrentVersion } from 'models/api/ConcurrentVersion';
export interface Api_Role extends Api_ConcurrentVersion {
  id?: number;
  roleUid?: string;
  keycloakGroupId?: string;
  name?: string;
  description?: string;
  isPublic: boolean;
  isDisabled: boolean;
}
