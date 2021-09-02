export enum AccountActive {
  YES = 'Yes',
  NO = 'No',
}

export interface IUserRecord {
  id?: number;
  keycloakUserId?: string;
  email?: string;
  businessIdentifier?: string;
  firstName?: string;
  surname?: string;
  isDisabled?: boolean;
  organization?: string;
  roles?: string;
  position?: string;
  lastLogin?: string;
  createdOn?: string;
  rowVersion?: number;
}
