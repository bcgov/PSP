export interface ILookupCode {
  id: number;
  code?: string;
  name: string;
  isDisabled: boolean;
  isPublic?: boolean;
  isVisible?: boolean;
  type: string;
  parentId?: number;
  sortOrder?: number;
}
