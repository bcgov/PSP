import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import Claims from 'constants/claims';
import { Classifications } from 'constants/classifications';
import _ from 'lodash';
import { useCallback } from 'react';
import { useAppSelector } from 'store/hooks';
import { ILookupCode } from 'store/slices/lookupCodes';
import { mapLookupCode } from 'utils';

import { useKeycloakWrapper } from './useKeycloakWrapper';

/**
 * Hook to return an array ILookupCode for specific types.
 */
export function useLookupCodeHelpers() {
  const keycloak = useKeycloakWrapper();
  const lookupCodes = useAppSelector(state => state.lookupCode.lookupCodes);
  const getCodeById = (type: string, id: string): string | undefined => {
    return lookupCodes
      .filter((code: { type: string; id: string }) => code.type === type && code.id === id)
      ?.find((x: any) => x)?.code;
  };

  const getByType = useCallback(
    (type: string) =>
      lookupCodes.filter(
        (code: { type: string; isDisabled: boolean }) =>
          code.type === type && code.isDisabled !== true,
      ),
    [lookupCodes],
  );

  const getPublicByType = useCallback(
    (type: string) =>
      lookupCodes.filter(
        (code: ILookupCode) =>
          code.type === type && code.isDisabled === false && code.isPublic !== false,
      ),
    [lookupCodes],
  );

  /**
   * filter the passed list of agencies. If the passed agency is a parent agency, include all child agencies. Otherwise just return the filter agency.
   * @param codes
   * @param lookupCodeId
   */
  const filterByParent = (codes: ILookupCode[], lookupCodeId: number): ILookupCode[] => {
    const filterByCode = _.find(codes, { id: lookupCodeId }) as ILookupCode;
    if (filterByCode?.parentId !== undefined) {
      return [filterByCode];
    }
    if (filterByCode === undefined) {
      return [];
    }
    const filteredAgencies = _.filter(codes, { parentId: filterByCode?.id });
    filteredAgencies.unshift(filterByCode);
    return filteredAgencies as ILookupCode[];
  };

  const getOptionsByType = (type: string) => getByType(type).map(mapLookupCode);

  /**
   * Return an array of SelectOptions containing property classifications.
   * @param filter - A filter to determine which classifications will be returned.
   * @returns An array of SelectOptions for property classifications.
   */
  const getPropertyClassificationOptions = (
    filter?: (value: SelectOption, index: number, array: SelectOption[]) => unknown,
  ) => {
    const classifications = getByType(API.PROPERTY_CLASSIFICATION_CODE_SET_NAME);
    return filter
      ? (classifications ?? []).map((c: ILookupCode) => mapLookupCode(c)).filter(filter)
      : !keycloak.hasClaim(Claims.ADMIN_PROPERTIES)
      ? (classifications ?? [])
          .map((c: ILookupCode) => mapLookupCode(c))
          .filter(
            (c: { value: string | number }) =>
              +c.value !== Classifications.Demolished &&
              +c.value !== Classifications.Subdivided &&
              +c.value !== Classifications.Disposed,
          )
      : (classifications ?? []).map((c: ILookupCode) => mapLookupCode(c));
  };

  /**
   * Returns the full name of an agency or the short code if
   * the full name is not found
   * @param agencyCode the short code for the agency
   */
  const getAgencyFullName = (agencyCode?: string) => {
    const agencies = getByType(API.AGENCY_CODE_SET_NAME);
    const agencyItem = agencies.find(
      (listItem: { code: string | undefined }) => listItem.code === agencyCode,
    );
    return agencyItem ? agencyItem.name : agencyCode;
  };

  return {
    getOptionsByType,
    getPropertyClassificationOptions,
    getCodeById,
    getByType,
    getPublicByType,
    filterByParent,
    getAgencyFullName,
    lookupCodes,
  };
}

export default useLookupCodeHelpers;
