import { ApiGen_Concepts_Role } from '@/models/api/generated/ApiGen_Concepts_Role';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const getEmptyRole = (): ApiGen_Concepts_Role => {
  return {
    id: 0,
    roleUid: null,
    keycloakGroupId: null,
    name: null,
    description: null,
    isPublic: false,
    isDisabled: false,
    displayOrder: 0,
    roleClaims: null,
    ...getEmptyBaseAudit(),
  };
};
