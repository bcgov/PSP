import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ILookupCode } from '@/store/slices/lookupCodes';

export const defaultTypeCode = (): ApiGen_Base_CodeType<string> => ({
  id: '',
  description: '',
  isDisabled: false,
  displayOrder: null,
});

export class TypeCodeUtils {
  public static createFromLookup<T extends string | number>(
    lookupCode: ILookupCode,
  ): ApiGen_Base_CodeType<T> {
    return {
      id: lookupCode.id as T,
      description: lookupCode.name,
      displayOrder: lookupCode.displayOrder,
      isDisabled: lookupCode.isDisabled,
    };
  }
}
