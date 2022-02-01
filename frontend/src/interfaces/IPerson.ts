import { IAddress } from 'interfaces';

export interface IPerson {
  id?: string;
  fullName?: string;
  surname?: string;
  firstName?: string;
  middleNames?: string;
  mobile?: string;
  landline?: string;
  email?: string;
  address?: IAddress;
}
