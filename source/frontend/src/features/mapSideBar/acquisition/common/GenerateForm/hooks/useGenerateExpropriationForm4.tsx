import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { Api_GenerateExpropriationForm4 } from '@/models/generate/acquisition/GenerateExpropriationForm4';
import { isValidId } from '@/utils';

import { ExpropriationForm1Model } from '../../../tabs/expropriation/models';
import { GetExtraFieldsFn, useGenerateExpropriationForm } from './useGenerateExpropriationForm';

export const useGenerateExpropriationForm4 = () => {
  const { getOrganizationConcept } = useApiContacts();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();

  const getExtraFields: GetExtraFieldsFn = async (
    _formModel: ExpropriationForm1Model,
    _acquisitionFileId: number,
    _file: ApiGen_Concepts_File,
    _properties: ApiGen_Concepts_AcquisitionFileProperty[],
    interestHolders: ApiGen_Concepts_InterestHolder[],
    expAuthority: ApiGen_Concepts_Organization | null,
  ) => {
    return {
      interestHolders: interestHolders ?? [],
      expropriationAuthority: expAuthority ?? null,
    };
  };

  getExtraFields.interestHoldersPromise = acquisitionFileId =>
    getAcquisitionInterestHolders.execute(acquisitionFileId);
  getExtraFields.expAuthorityPromise = formModel =>
    isValidId(formModel.expropriationAuthority?.contact?.organizationId)
      ? getOrganizationConcept(formModel.expropriationAuthority.contact.organizationId)
      : Promise.resolve(null);

  return useGenerateExpropriationForm(
    ApiGen_CodeTypes_FormTypes.FORM4.toString(),
    Api_GenerateExpropriationForm4,
    getExtraFields,
  );
};
