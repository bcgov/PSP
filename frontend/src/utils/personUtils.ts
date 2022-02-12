import { IEditablePerson } from 'interfaces/editable-contact';

export function formatFullName(person?: Partial<IEditablePerson>): string {
  if (!person) {
    return '';
  }
  const nameParts = [person.firstName, person.middleNames, person.surname];
  const fullName = nameParts.filter(n => n != null && n.trim() !== '').join(' ');
  return fullName;
}
