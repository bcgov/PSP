import { InterestHolderType } from '@/constants/interestHolderTypes';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { Api_GenerateExpropriationForm1 } from '@/models/generate/acquisition/GenerateExpropriationForm1';
import { isValidId } from '@/utils';

import { ExpropriationForm1Model } from '../../../tabs/expropriation/models';
import { GetExtraFieldsFn, useGenerateExpropriationForm } from './useGenerateExpropriationForm';

export const useGenerateExpropriationForm1 = () => {
  const { getOrganizationConcept, getPersonConcept } = useApiContacts();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();

  // Provide promise creators for the generic hook
  const getExtraFields: GetExtraFieldsFn = async (
    formModel: ExpropriationForm1Model,
    _acquisitionFileId: number,
    _file: ApiGen_Concepts_File,
    _properties: ApiGen_Concepts_AcquisitionFileProperty[],
    interestHolders: ApiGen_Concepts_InterestHolder[],
    expAuthority: ApiGen_Concepts_Organization | null,
  ) => {
    // Fetch primary contact information for organizations within interest holders (in parallel)
    if (interestHolders) {
      await Promise.all(
        interestHolders.map(async holder => {
          const primaryContactPerson =
            isValidId(holder?.organizationId) && isValidId(holder?.primaryContactId)
              ? (await getPersonConcept(holder?.primaryContactId))?.data
              : null;
          holder.primaryContact = primaryContactPerson;
        }),
      );
    }

    const ownerSolicitor = interestHolders?.find(
      x => x.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR,
    );
    return {
      interestHolders: interestHolders ?? [],
      expropriationAuthority: expAuthority ?? null,
      ownerSolicitor: ownerSolicitor ?? null,
      landInterest: formModel?.landInterest,
      purpose: formModel?.purpose,
    };
  };

  // Attach promise creators for interest holders and exp authority
  getExtraFields.interestHoldersPromise = (acquisitionFileId: number) =>
    getAcquisitionInterestHolders.execute(acquisitionFileId);
  getExtraFields.expAuthorityPromise = (formModel: ExpropriationForm1Model) =>
    isValidId(formModel.expropriationAuthority?.contact?.organizationId)
      ? getOrganizationConcept(formModel.expropriationAuthority.contact.organizationId)
      : Promise.resolve(null);

  return useGenerateExpropriationForm(
    ApiGen_CodeTypes_FormTypes.FORM1.toString(),
    Api_GenerateExpropriationForm1,
    getExtraFields,
  );
};
