import * as API from 'constants/API';
import { CountryCodes } from 'constants/countryCodes';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Dictionary } from 'interfaces/Dictionary';
import { useCallback, useMemo } from 'react';
import { ILookupCode } from 'store/slices/lookupCodes';

/**
 * Extending default ILookupCode interface to enforce the presence of `displayOrder` property
 * This is okay here because both code tables have this field (not the case with other code tables)
 */
interface IHasDisplayOrder extends Omit<ILookupCode, 'displayOrder'> {
  displayOrder: number;
}

const fieldLabels = new Map<string, Dictionary<string>>([
  [CountryCodes.Canada, { province: 'Province', postal: 'Postal Code' }],
  [CountryCodes.US, { province: 'State', postal: 'Zip Code' }],
  [CountryCodes.Other, { province: '', postal: 'Postal Code' }],
]);

/**
 * Hook that provides several helpers to the Address component.
 */
export function useAddressHelpers() {
  const { getByType } = useLookupCodeHelpers();

  const countries = useMemo(() => {
    const unsorted = getByType(API.COUNTRY_TYPES) as IHasDisplayOrder[];
    return unsorted.sort(byDisplayOrder);
  }, [getByType]);

  const provinces = useMemo(() => {
    const unsorted = getByType(API.PROVINCE_TYPES) as IHasDisplayOrder[];
    return unsorted.sort(byDisplayOrder);
  }, [getByType]);

  const getFieldLabel = useCallback((field: string, countryCode = CountryCodes.Canada) => {
    const entry =
      fieldLabels.get(countryCode) || (fieldLabels.get(CountryCodes.Canada) as Dictionary<string>);
    return entry[field] || '';
  }, []);

  return { countries, provinces, getFieldLabel };
}

const byDisplayOrder = (a: IHasDisplayOrder, b: IHasDisplayOrder) =>
  a.displayOrder - b.displayOrder;

export default useAddressHelpers;
