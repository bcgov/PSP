export interface IUserInfo {
  id: number;
  keycloakUserId: string;
  businessIdentifier: string;
  name: string;
  email: string;
  firstName: string;
  surname: string;
  Groups: any[];
  organizations: number[];
  position?: string;
}
