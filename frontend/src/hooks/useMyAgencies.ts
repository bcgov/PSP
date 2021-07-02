import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { useMemo } from 'react';

import useKeycloakWrapper from './useKeycloakWrapper';
import useLookupCodeHelpers from './useLookupCodeHelpers';

/**
 * Hook to get only the agencies that the user belongs to
 * Parent Agency => Parent agency + child agencies
 * Child Agency => only the child agency
 * @returns array of agency select options
 */
export const useMyAgencies = (): SelectOption[] => {
  const { getOptionsByType } = useLookupCodeHelpers();

  const keycloak = useKeycloakWrapper();
  const agencies = getOptionsByType(API.AGENCY_CODE_SET_NAME);
  const userAgency = agencies.find(a => Number(a.value) === Number(keycloak.agencyId));

  const agencyOptions = useMemo(() => {
    return agencies.filter(a => {
      return (
        Number(a.value) === Number(userAgency?.value) ||
        Number(a.parentId) === Number(userAgency?.value)
      );
    });
  }, [userAgency, agencies]);

  return agencyOptions;
};
