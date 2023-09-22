import { IContactSearchResult } from '@/interfaces';
import { IEditablePerson } from '@/interfaces/editable-contact';
import { Api_Person } from '@/models/api/Person';

export function formatFullName(person?: Partial<IEditablePerson>): string {
  if (!person) {
    return '';
  }
  return formatNames([person.firstName, person.middleNames, person.surname]);
}

export function formatApiPersonNames(person?: Api_Person | null): string {
  return formatNames([person?.firstName, person?.middleNames, person?.surname]);
}

export function formatNames(nameParts: Array<string | undefined | null>): string {
  return nameParts.filter(n => n !== null && n !== undefined && n.trim() !== '').join(' ');
}

export function formatContactSearchResult(
  contact: IContactSearchResult,
  defaultText: string = '',
): string {
  let text = defaultText;
  if (contact?.personId !== undefined) {
    text = formatNames([contact.firstName, contact.middleNames, contact.surname]);
  } else if (contact?.organizationId !== undefined) {
    text = contact.organizationName || '';
  }
  return text;
}
