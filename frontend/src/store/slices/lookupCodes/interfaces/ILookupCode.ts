export interface ILookupCode {
  id: number | string;
  code?: string;
  name: string;
  isDisabled: boolean;
  isPublic?: boolean;
  isVisible?: boolean;
  type: string;
  parentId?: number;
  key?: string;
  description?: string;
  displayOrder?: number;
  appCreateTimestamp?: string;
  updatedOn?: string;
  updatedByName?: string;
  rowVersion?: number;
}
