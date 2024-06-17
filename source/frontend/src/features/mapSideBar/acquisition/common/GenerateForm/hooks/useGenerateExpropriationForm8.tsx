import moment from 'moment';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useForm8Repository } from '@/hooks/repositories/useForm8Repository';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { Api_GenerateExpropriationForm8 } from '@/models/generate/acquisition/GenerateExpropriationForm8';
import { stringDate } from '@/models/layers/alcAgriculturalReserve';

export const useGenerateExpropriationForm8 = () => {
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const {
    getForm8: { execute: getForm8 },
  } = useForm8Repository();

  const generateForm8 = async (form8Id: number, acquisitionFileNumber: stringDate) => {
    const form8Api = await getForm8(form8Id);

    if (form8Api) {
      const formData = new Api_GenerateExpropriationForm8(form8Api);
      const generatedFile = await generate({
        templateType: ApiGen_CodeTypes_FormTypes.FORM8.toString(),
        templateData: formData,
        convertToType: null,
      });

      if (
        generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
        generatedFile?.payload
      ) {
        const fileExt = generatedFile?.payload?.fileNameExtension ?? 'docx';
        const fileName = `Form 8-${acquisitionFileNumber}-${moment().format(
          'yyyyMMDD_hhmmss',
        )}.${fileExt}`;
        createFileDownload(generatedFile?.payload, fileName);
      } else {
        throw Error('Failed to generate file');
      }
    }
  };

  return generateForm8;
};
