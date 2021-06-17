export interface ILookupCode {
  code: string;
  name: string;
  id: number;
  isDisabled: boolean;
  isPublic?: boolean;
  isVisible?: boolean;
  type: string;
  parentId?: number;
  sortOrder?: number;
}
