import moment from 'moment';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ExpropriationForm9Model } from '@/features/mapSideBar/acquisition/tabs/expropriation/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateExpropriationForm9 } from '@/models/generate/acquisition/GenerateExpropriationForm9';
import { isValidId } from '@/utils';

export const useGenerateExpropriationForm9 = () => {
  const { getOrganizationConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateForm5 = async (acquisitionFileId: number, formModel: ExpropriationForm9Model) => {
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

    const fileData = new Api_GenerateAcquisitionFile({
      file: file,
      interestHolders: interestHolders ?? [],
    });

    const filePropertyIds = new Set(
      formModel.impactedProperties.map(fp => fp?.id).filter(isValidId),
    );
    const selectedProperties = properties?.filter(fp => filePropertyIds.has(Number(fp.id)));

    const expropriationData = new Api_GenerateExpropriationForm9({
      file: fileData,
      interestHolders: interestHolders ?? [],
      expropriationAuthority: expAuthority?.data ?? null,
      impactedProperties: selectedProperties,
      registeredPlanNumbers: formModel?.registeredPlanNumbers ?? '',
    });

    const generatedFile = await generate({
      templateType: ApiGen_CodeTypes_FormTypes.FORM9.toString(),
      templateData: expropriationData,
      convertToType: null,
    });
    if (
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
      generatedFile?.payload
    ) {
      const fileExt = generatedFile?.payload?.fileNameExtension ?? 'docx';
      const fileName = `Form 9-${file.fileNumber}-${moment().format('yyyyMMDD_hhmmss')}.${fileExt}`;
      createFileDownload(generatedFile?.payload, fileName);
    } else {
      throw Error('Failed to generate file');
    }
  };

  return generateForm5;
};
