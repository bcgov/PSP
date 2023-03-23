export interface ILookupCode {
  id: string | number;
  code?: string;
  name: string;
  isDisabled: boolean;
  isPublic?: boolean;
  isVisible?: boolean;
  type: string;
  parentId?: string | number;
  key?: string;
  description?: string;
  hint?: string;
  displayOrder: number;
  appCreateTimestamp?: string;
  updatedOn?: string;
  updatedByName?: string;
  rowVersion?: number;
}
