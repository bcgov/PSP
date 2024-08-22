import { useCallback } from 'react';

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

  return {
    getOptionsByType,
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
