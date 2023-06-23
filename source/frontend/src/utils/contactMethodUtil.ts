import { ContactMethodTypes } from '@/constants/contactMethodType';
import { Api_ContactMethod } from '@/models/api/ContactMethod';

export function getPreferredContactMethodValue(
  methods: Api_ContactMethod[] | undefined,
  ...typeCodes: ContactMethodTypes[]
): string | undefined {
  if (methods === undefined) {
    return undefined;
  }

  const stringTypeCodes = typeCodes.map(type => type.toString());
  return (
    methods.find(
      x => stringTypeCodes.includes(x.contactMethodType?.id ?? '') && x.isPreferredMethod,
    )?.value ?? methods.find(x => stringTypeCodes.includes(x.contactMethodType?.id ?? ''))?.value
  );
}
