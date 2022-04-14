import { ContactMethodTypes } from 'constants/contactMethodType';
import { Api_ContactMethod } from 'models/api/ContactMethod';

export function getContactMethodValue(
  methods: Api_ContactMethod[] | undefined,
  typeCode: ContactMethodTypes,
): string | undefined {
  if (methods === undefined) {
    return undefined;
  }

  return methods.find(x => x.contactMethodType?.id === typeCode)?.value;
}
