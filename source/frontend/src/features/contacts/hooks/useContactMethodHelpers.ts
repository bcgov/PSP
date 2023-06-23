import { useMemo } from 'react';

import * as API from '@/constants/API';
import {
  ContactMethodTypes,
  EmailContactMethods,
  PhoneContactMethods,
} from '@/constants/contactMethodType';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ILookupCode } from '@/store/slices/lookupCodes';

/**
 * Extending default ILookupCode interface to enforce the presence of `displayOrder` property
 * This is okay here because both code tables have this field (not the case with other code tables)
 */
interface IHasDisplayOrder extends Omit<ILookupCode, 'displayOrder'> {
  displayOrder: number;
}

/**
 * Hook that provides several helpers to the Contact Info component.
 */
export function useContactMethodHelpers() {
  const { getByType } = useLookupCodeHelpers();

  const phoneTypes = useMemo(() => {
    const unsorted = getByType(API.CONTACT_METHOD_TYPES).filter(x =>
      PhoneContactMethods.includes(x.id as ContactMethodTypes),
    );

    return (unsorted as IHasDisplayOrder[]).sort(byDisplayOrder);
  }, [getByType]);

  const emailTypes = useMemo(() => {
    const unsorted = getByType(API.CONTACT_METHOD_TYPES).filter(x =>
      EmailContactMethods.includes(x.id as ContactMethodTypes),
    );

    return (unsorted as IHasDisplayOrder[]).sort(byDisplayOrder);
  }, [getByType]);

  return { phoneTypes, emailTypes };
}

const byDisplayOrder = (a: IHasDisplayOrder, b: IHasDisplayOrder) =>
  a.displayOrder - b.displayOrder;

export default useContactMethodHelpers;
