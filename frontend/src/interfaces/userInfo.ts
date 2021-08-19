export interface IUserInfo {
  id: number;
  key: string;
  username: string;
  name: string;
  email: string;
  firstName: string;
  lastName: string;
  Groups: any[];
  organizations: number[];
  position?: string;
}
