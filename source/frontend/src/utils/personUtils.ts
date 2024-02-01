import { IEditablePerson } from '@/interfaces/editable-contact';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';

import { exists } from './utils';

export function formatFullName(person?: Partial<IEditablePerson>): string {
  if (!person) {
    return '';
  }
  return formatNames([person.firstName, person.middleNames, person.surname]);
}

export function formatApiPersonNames(person: ApiGen_Concepts_Person | null | undefined): string {
  return formatNames([person?.firstName, person?.middleNames, person?.surname]);
}

export function formatNames(nameParts: Array<string | undefined | null>): string {
  return nameParts.filter(n => exists(n) && n.trim() !== '').join(' ');
}
