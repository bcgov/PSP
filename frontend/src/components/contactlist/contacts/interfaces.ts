export interface IContactFilter {
  summary: string;
  municipality: string;
  activeContactsOnly: boolean;
  searchBy: string;
}

export enum ContactTypes {
  ORGANIZATION = 'O',
  INDIVIDUAL = 'P',
}
