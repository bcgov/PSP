import { RestrictContactType } from './ContactFilterComponent/ContactFilterComponent';

export interface IContactFilter {
  summary: string;
  municipality: string;
  activeContactsOnly: boolean;
  searchBy: RestrictContactType[];
}
