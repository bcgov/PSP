import moment from 'moment';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ExpropriationForm6Model } from '@/features/mapSideBar/acquisition/tabs/expropriation/models';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateExpropriationForm6 } from '@/models/generate/acquisition/GenerateExpropriationForm6';
import { isValidId } from '@/utils';

export const useGenerateExpropriationForm6 = () => {
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateForm6 = async (acquisitionFileId: number, formModel: ExpropriationForm6Model) => {
    const filePromise = getAcquisitionFile.execute(acquisitionFileId);
    const propertiesPromise = getAcquisitionProperties.execute(acquisitionFileId);

    const [file, properties] = await Promise.all([filePromise, propertiesPromise]);
    if (!file) throw Error('Acquisition file not found');
    file.fileProperties = properties ?? null;

    const fileData = new Api_GenerateAcquisitionFile({
      file: file,
      interestHolders: [],
    });

    const filePropertyIds = new Set(
      formModel.impactedProperties.map(fp => fp?.id).filter(isValidId),
    );
    const selectedProperties = properties?.filter(fp => filePropertyIds.has(Number(fp.id)));

    const expropriationData = new Api_GenerateExpropriationForm6({
      file: fileData,
      interestHolders: [],
      expropriationAuthority: null,
      impactedProperties: selectedProperties,
    });

    const generatedFile = await generate({
      templateType: 'FORM6',
      templateData: expropriationData,
      convertToType: null,
    });

    if (
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
      generatedFile?.payload
    ) {
      const fileExt = generatedFile?.payload?.fileNameExtension ?? 'docx';
      const fileName = `Form 6-${file.fileNumber}-${moment().format('yyyyMMDD_hhmmss')}.${fileExt}`;
      createFileDownload(generatedFile?.payload, fileName);
    } else {
      throw Error('Failed to generate file');
    }
  };

  return generateForm6;
};
