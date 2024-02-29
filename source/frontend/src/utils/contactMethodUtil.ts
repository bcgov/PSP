import { ContactMethodTypes } from '@/constants/contactMethodType';
import { ApiGen_Concepts_ContactMethod } from '@/models/api/generated/ApiGen_Concepts_ContactMethod';

import { exists } from './utils';

export function getPreferredContactMethodValue(
  methods: ApiGen_Concepts_ContactMethod[] | null | undefined,
  ...typeCodes: ContactMethodTypes[]
): string | null {
  if (!exists(methods)) {
    return null;
  }

  const stringTypeCodes = typeCodes.map(type => type.toString());
  return methods.find(x => stringTypeCodes.includes(x.contactMethodType?.id ?? ''))?.value ?? null;
}
