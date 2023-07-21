import { ConvertToTypes } from '@/constants/convertToTypes';
import { InterestHolderType } from '@/constants/interestHolderTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ExpropriationForm1Model } from '@/features/mapSideBar/acquisition/tabs/expropriation/models';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { ExternalResultStatus } from '@/models/api/ExternalResult';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateExpropriationForm1 } from '@/models/generate/acquisition/GenerateExpropriationForm1';

export const useGenerateExpropriationForm1 = () => {
  const { getOrganizationConcept, getPersonConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateForm1 = async (acquisitionFileId: number, form1: ExpropriationForm1Model) => {
    const filePromise = getAcquisitionFile.execute(acquisitionFileId);
    const propertiesPromise = getAcquisitionProperties.execute(acquisitionFileId);
    const interestHoldersPromise = getAcquisitionInterestHolders.execute(acquisitionFileId);
    const expropriationAuthorityPromise = form1.expropriationAuthority?.contact?.organizationId
      ? getOrganizationConcept(form1.expropriationAuthority.contact.organizationId)
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
    file.fileProperties = properties;

    const ownerSolicitor = file.acquisitionFileInterestHolders?.find(
      x => x.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR,
    );

    const ownerSolicitorPerson = ownerSolicitor?.personId
      ? (await getPersonConcept(ownerSolicitor?.personId))?.data
      : null;

    const fileData = new Api_GenerateAcquisitionFile({
      file: file,
      interestHolders: interestHolders ?? [],
      ownerSolicitor: ownerSolicitorPerson ?? null,
    });

    const filePropertyIds = new Set(
      form1.impactedProperties.map(fp => fp?.id).filter((p): p is number => !!p),
    );
    const selectedProperties = properties?.filter(fp => filePropertyIds.has(Number(fp.id)));

    const expropriationData = new Api_GenerateExpropriationForm1({
      file: fileData,
      interestHolders: interestHolders ?? [],
      expropriationAuthority: expAuthority?.data ?? null,
      impactedProperties: selectedProperties,
      landInterest: form1?.landInterest,
      purpose: form1?.purpose,
    });

    const generatedFile = await generate({
      templateType: FormTemplateTypes.EXPROP_FORM_1,
      templateData: expropriationData,
      convertToType: ConvertToTypes.PDF,
    });
    if (generatedFile?.status === ExternalResultStatus.Success && generatedFile?.payload) {
      showFile(generatedFile?.payload);
    } else {
      throw Error('Failed to generate file');
    }
  };

  return generateForm1;
};
