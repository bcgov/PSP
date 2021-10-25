import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { useCallback } from 'react';
import { useAppSelector } from 'store/hooks';
import { ILookupCode } from 'store/slices/lookupCodes';
import { mapLookupCode } from 'utils';

/**
 * Hook to return an array ILookupCode for specific types.
 */
export function useLookupCodeHelpers() {
  const lookupCodes = useAppSelector(state => state.lookupCode.lookupCodes);
  const getCodeById = (type: string, id: number | string): string | undefined => {
    const match = lookupCodes
      .filter((code: { type: string; id: number | string }) => code.type === type && code.id === id)
      ?.find((x: any) => x);
    return match?.code ?? match?.name;
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

  const getOptionsByType = (type: string) => getByType(type).map(mapLookupCode);

  /**
   * Return an array of SelectOptions containing property classifications.
   * @param filter - A filter to determine which classifications will be returned.
   * @returns An array of SelectOptions for property classifications.map
   */
  const getPropertyClassificationTypeOptions = (
    filter?: (value: SelectOption, index: number, array: SelectOption[]) => unknown,
  ) => {
    const classifications = getByType(API.PROPERTY_CLASSIFICATION_CODE_SET_NAME);
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

export default useLookupCodeHelpers;
