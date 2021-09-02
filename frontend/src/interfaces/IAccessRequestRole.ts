export interface IAccessRequestRole {
  id: number;
  name?: string;
  description?: string;
  isDisabled?: boolean;
  sortOrder?: number;
  createdOn?: string;
  updatedByName?: string;
  updatedByEmail?: string;
  rowVersion?: number;
}
