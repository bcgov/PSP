import { RestrictContactType } from '@/constants/contacts';

export interface IContactFilter {
  summary: string;
  municipality: string;
  activeContactsOnly: boolean;
  searchBy: RestrictContactType[];
}
