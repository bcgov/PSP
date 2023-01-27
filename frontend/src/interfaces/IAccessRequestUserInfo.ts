export interface IAccessRequestUserInfo {
  id: number;
  keycloakUserId: string;
  username: string;
  name: string;
  email: string;
  firstName: string;
  family_name: string;
  position?: string;
}
