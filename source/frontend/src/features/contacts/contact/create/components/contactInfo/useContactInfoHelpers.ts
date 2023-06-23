import { useMemo } from 'react';

import * as API from '@/constants/API';
import { EmailContactMethods, PhoneContactMethods } from '@/constants/contactMethodType';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

/**
 * Hook that provides several helpers to the ContactPhone / ContactEmail components.
 */
export default function useContactInfoHelpers() {
  const { getOptionsByType } = useLookupCodeHelpers();

  const phoneTypes = useMemo(
    () =>
      getOptionsByType(API.CONTACT_METHOD_TYPES).filter(
        c => c.value && PhoneContactMethods.includes(c.value as string),
      ),
    [getOptionsByType],
  );

  const emailTypes = useMemo(
    () =>
      getOptionsByType(API.CONTACT_METHOD_TYPES).filter(
        c => c.value && EmailContactMethods.includes(c.value as string),
      ),
    [getOptionsByType],
  );

  return { phoneTypes, emailTypes };
}
