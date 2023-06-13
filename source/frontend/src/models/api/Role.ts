import { Api_AuditFields } from '@/models/api/AuditFields';
import { Api_ConcurrentVersion } from '@/models/api/ConcurrentVersion';

export interface Api_Role extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  roleUid?: string;
  keycloakGroupId?: string;
  name?: string;
  description?: string;
  isPublic: boolean;
  isDisabled: boolean;
  displayOrder?: number;
  roleClaims?: any[];
}
