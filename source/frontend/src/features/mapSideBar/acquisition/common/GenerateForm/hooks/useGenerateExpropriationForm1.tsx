import moment from 'moment';

import { InterestHolderType } from '@/constants/interestHolderTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ExpropriationForm1Model } from '@/features/mapSideBar/acquisition/tabs/expropriation/models';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateExpropriationForm1 } from '@/models/generate/acquisition/GenerateExpropriationForm1';
import { isValidId } from '@/utils';

export const useGenerateExpropriationForm1 = () => {
  const { getOrganizationConcept, getPersonConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateForm1 = async (acquisitionFileId: number, formModel: ExpropriationForm1Model) => {
    const filePromise = getAcquisitionFile.execute(acquisitionFileId);
    const propertiesPromise = getAcquisitionProperties.execute(acquisitionFileId);
    const interestHoldersPromise = getAcquisitionInterestHolders.execute(acquisitionFileId);
    const expropriationAuthorityPromise = formModel.expropriationAuthority?.contact?.organizationId
      ? getOrganizationConcept(formModel.expropriationAuthority.contact.organizationId)
      : Promise.resolve(null);

    const [file, properties, interestHolders, expAuthority] = await Promise.all([
      filePromise,
      propertiesPromise,
      interestHoldersPromise,
      expropriationAuthorityPromise,
    ]);
    if (!file) {
      throw Error('Acquisition file not found');
    }
    file.fileProperties = properties ?? null;

    // fetch primary contact information for organizations within interest holders
    if (interestHolders) {
      await Promise.all(
        interestHolders.map(async holder => {
          const primaryContactPerson =
            holder?.organizationId && holder?.primaryContactId
              ? (await getPersonConcept(holder?.primaryContactId))?.data
              : null;
          holder.primaryContact = primaryContactPerson;
        }),
      );
    }

    const ownerSolicitor = interestHolders?.find(
      x => x.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR,
    );

    const fileData = new Api_GenerateAcquisitionFile({
      file: file,
      interestHolders: interestHolders ?? [],
      ownerSolicitor: ownerSolicitor ?? null,
    });

    const filePropertyIds = new Set(
      formModel.impactedProperties.map(fp => fp?.id).filter(isValidId),
    );
    const selectedProperties = properties?.filter(fp => filePropertyIds.has(Number(fp.id)));

    const expropriationData = new Api_GenerateExpropriationForm1({
      file: fileData,
      interestHolders: interestHolders ?? [],
      expropriationAuthority: expAuthority?.data ?? null,
      impactedProperties: selectedProperties,
      landInterest: formModel?.landInterest,
      purpose: formModel?.purpose,
    });

    const generatedFile = await generate({
      templateType: FormTemplateTypes.EXPROP_FORM_1,
      templateData: expropriationData,
      convertToType: null,
    });

    if (
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
      generatedFile?.payload
    ) {
      const fileExt = generatedFile?.payload?.fileNameExtension ?? 'docx';
      const fileName = `Form 1-${file.fileNumber}-${moment().format('yyyyMMDD_hhmmss')}.${fileExt}`;
      showFile(generatedFile?.payload, fileName);
    } else {
      throw Error('Failed to generate file');
    }
  };

  return generateForm1;
};
