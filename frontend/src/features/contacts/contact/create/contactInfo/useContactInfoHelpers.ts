import * as API from 'constants/API';
import { emailContactMethods, phoneContactMethods } from 'constants/contactMethodType';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { useMemo } from 'react';

/**
 * Hook that provides several helpers to the ContactPhone / ContactEmail components.
 */
export default function useContactInfoHelpers() {
  const { getOptionsByType } = useLookupCodeHelpers();

  const phoneTypes = useMemo(
    () =>
      getOptionsByType(API.CONTACT_METHOD_TYPES).filter(
        c => c.value && phoneContactMethods.includes(c.value as string),
      ),
    [getOptionsByType],
  );

  const emailTypes = useMemo(
    () =>
      getOptionsByType(API.CONTACT_METHOD_TYPES).filter(
        c => c.value && emailContactMethods.includes(c.value as string),
      ),
    [getOptionsByType],
  );

  return { phoneTypes, emailTypes };
}
