import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { useMemo } from 'react';

import useKeycloakWrapper from './useKeycloakWrapper';
import useLookupCodeHelpers from './useLookupCodeHelpers';

/**
 * Hook to get only the organizations that the user belongs to
 * Parent Organization => Parent organization + child organizations
 * Child Organization => only the organization
 * @returns array of organization select options
 */
export const useMyOrganizations = (): SelectOption[] => {
  const { getOptionsByType } = useLookupCodeHelpers();

  const keycloak = useKeycloakWrapper();
  const organizations = getOptionsByType(API.ORGANIZATION_CODE_SET_NAME);
  const userOrganization = organizations.find(
    a => Number(a.value) === Number(keycloak.organizationId),
  );

  const organizationOptions = useMemo(() => {
    return organizations.filter(a => {
      return (
        Number(a.value) === Number(userOrganization?.value) ||
        Number(a.parentId) === Number(userOrganization?.value)
      );
    });
  }, [userOrganization, organizations]);

  return organizationOptions;
};
