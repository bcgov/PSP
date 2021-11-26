export enum AccountActive {
  YES = 'Yes',
  NO = 'No',
}

export interface IUserRecord {
  id?: number;
  keycloakUserId?: string;
  email?: string;
  businessIdentifierValue?: string;
  firstName?: string;
  surname?: string;
  isDisabled?: boolean;
  organization?: string;
  roles?: string;
  position?: string;
  lastLogin?: string;
  appCreateTimestamp?: string;
  rowVersion?: number;
}
