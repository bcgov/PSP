import { IEditablePerson } from 'interfaces/editable-contact';

export function formatFullName(person?: Partial<IEditablePerson>): string {
  if (!person) {
    return '';
  }
  return formatNames([person.firstName, person.middleNames, person.surname]);
}

export function formatNames(nameParts: Array<string | undefined | null>): string {
  return nameParts.filter(n => n !== null && n !== undefined && n.trim() !== '').join(' ');
}
