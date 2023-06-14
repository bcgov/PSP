import { ILookupCode } from '@/store/slices/lookupCodes';

export default interface ITypeCode<T> {
  id: T;
  description?: string;
  isDisabled?: boolean;
  displayOrder?: number;
}

export const defaultTypeCode: ITypeCode<string> = {
  id: '',
  description: '',
  isDisabled: false,
};

export class TypeCodeUtils {
  public static createFromLookup<T extends string | number>(lookupCode: ILookupCode): ITypeCode<T> {
    return {
      id: lookupCode.id as T,
      description: lookupCode.name,
      displayOrder: lookupCode.displayOrder,
      isDisabled: lookupCode.isDisabled,
    };
  }
}
