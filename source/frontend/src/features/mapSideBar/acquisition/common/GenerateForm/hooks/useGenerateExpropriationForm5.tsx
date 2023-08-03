import moment from 'moment';

import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ExpropriationForm5Model } from '@/features/mapSideBar/acquisition/tabs/expropriation/models';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { ExternalResultStatus } from '@/models/api/ExternalResult';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateExpropriationForm5 } from '@/models/generate/acquisition/GenerateExpropriationForm5';

export const useGenerateExpropriationForm5 = () => {
  const { getOrganizationConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateForm5 = async (acquisitionFileId: number, formModel: ExpropriationForm5Model) => {
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
    file.fileProperties = properties;

    const fileData = new Api_GenerateAcquisitionFile({
      file: file,
      interestHolders: interestHolders ?? [],
    });

    const filePropertyIds = new Set(
      formModel.impactedProperties.map(fp => fp?.id).filter((p): p is number => !!p),
    );
    const selectedProperties = properties?.filter(fp => filePropertyIds.has(Number(fp.id)));

    const expropriationData = new Api_GenerateExpropriationForm5({
      file: fileData,
      interestHolders: interestHolders ?? [],
      expropriationAuthority: expAuthority?.data ?? null,
      impactedProperties: selectedProperties,
    });

    const generatedFile = await generate({
      templateType: FormTemplateTypes.EXPROP_FORM_5,
      templateData: expropriationData,
      convertToType: null,
    });
    if (generatedFile?.status === ExternalResultStatus.Success && generatedFile?.payload) {
      const fileExt = generatedFile?.payload?.fileNameExtension ?? 'docx';
      const fileName = `Form 5-${file.fileNumber}-${moment().format('yyyyMMDD_hhmmss')}.${fileExt}`;
      showFile(generatedFile?.payload, fileName);
    } else {
      throw Error('Failed to generate file');
    }
  };

  return generateForm5;
};
