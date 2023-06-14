import { useCallback } from 'react';

import { SelectOption } from '@/components/common/form';
import * as API from '@/constants/API';
import { useAppSelector } from '@/store/hooks';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { mapLookupCode } from '@/utils';

/**
 * Hook to return an array ILookupCode for specific types.
 */
export function useLookupCodeHelpers() {
  const lookupCodes = useAppSelector(state => state.lookupCode?.lookupCodes ?? []);

  const getCodeById = (type: string, id: number | string): string | undefined => {
    const match = (lookupCodes || [])
      .filter((code: { type: string; id: number | string }) => code.type === type && code.id === id)
      ?.find((x: any) => x);
    return match?.code ?? match?.name;
  };

  const getByType = useCallback(
    (type: string, includeDisabled = false) =>
      (lookupCodes || [])
        .filter(code => code.type === type && (includeDisabled ? true : code.isDisabled !== true))
        .sort(byDisplayOrder),
    [lookupCodes],
  );

  const getPublicByType = useCallback(
    (type: string) =>
      (lookupCodes || []).filter(
        (code: ILookupCode) =>
          code.type === type && code.isDisabled === false && code.isPublic !== false,
      ),
    [lookupCodes],
  );

  const getOptionsByType = useCallback(
    (type: string, includeDisabled = false) => getByType(type, includeDisabled).map(mapLookupCode),
    [getByType],
  );

  /**
   * Return an array of SelectOptions containing property classifications.
   * @param filter - A filter to determine which classifications will be returned.
   * @returns An array of SelectOptions for property classifications.map
   */
  const getPropertyClassificationTypeOptions = (
    filter?: (value: SelectOption, index: number, array: SelectOption[]) => unknown,
  ) => {
    const classifications = getByType(API.PROPERTY_CLASSIFICATION_TYPES);
    return filter
      ? (classifications ?? []).map((c: ILookupCode) => mapLookupCode(c)).filter(filter)
      : (classifications ?? []).map((c: ILookupCode) => mapLookupCode(c));
  };

  return {
    getOptionsByType,
    getPropertyClassificationTypeOptions,
    getCodeById,
    getByType,
    getPublicByType,
    lookupCodes,
  };
}

function byDisplayOrder(a: ILookupCode, b: ILookupCode) {
  return a.displayOrder - b.displayOrder;
}

export default useLookupCodeHelpers;
