export interface IUserInfo {
  id: number;
  keycloakUserId: string;
  businessIdentifierValue: string;
  name: string;
  email: string;
  firstName: string;
  surname: string;
  Groups: any[];
  organizations: number[];
  position?: string;
}
