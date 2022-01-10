import { IAddress } from 'interfaces';

export interface IPerson {
  fullName?: string;
  surname?: string;
  firstName?: string;
  middleNames?: string;
  mobile?: string;
  landline?: string;
  email?: string;
  address?: IAddress;
}
