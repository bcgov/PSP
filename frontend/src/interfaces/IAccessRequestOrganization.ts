export interface IAccessRequestOrganization {
  id: number;
  code?: string;
  name?: string;
  description?: string;
  isDisabled?: boolean;
  sortOrder?: number;
  appCreateTimestamp?: string;
}
