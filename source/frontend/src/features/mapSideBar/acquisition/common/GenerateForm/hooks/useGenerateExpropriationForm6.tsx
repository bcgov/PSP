import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { Api_GenerateExpropriationFormBase } from '@/models/generate/acquisition/GenerateExpropriationFormBase';

import { useGenerateExpropriationForm } from './useGenerateExpropriationForm';

export const useGenerateExpropriationForm6 = () => {
  return useGenerateExpropriationForm(
    ApiGen_CodeTypes_FormTypes.FORM6.toString(),
    Api_GenerateExpropriationFormBase,
  );
};
