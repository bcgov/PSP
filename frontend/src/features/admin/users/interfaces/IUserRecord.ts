export enum AccountActive {
  YES = 'Yes',
  NO = 'No',
}

export interface IUserRecord {
  id: number;
  key: string;
  email?: string;
  username?: string;
  firstName?: string;
  lastName?: string;
  isDisabled?: boolean;
  agency?: string;
  roles?: string;
  position?: string;
  lastLogin?: string;
  createdOn?: string;
  rowVersion: number;
}
