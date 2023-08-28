import moment from 'moment';

import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useForm8Repository } from '@/hooks/repositories/useForm8Repository';
import { ExternalResultStatus } from '@/models/api/ExternalResult';
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
        templateType: FormTemplateTypes.EXPROP_FORM_8,
        templateData: formData,
        convertToType: null,
      });

      if (generatedFile?.status === ExternalResultStatus.Success && generatedFile?.payload) {
        const fileExt = generatedFile?.payload?.fileNameExtension ?? 'docx';
        const fileName = `Form 8-${acquisitionFileNumber}-${moment().format(
          'yyyyMMDD_hhmmss',
        )}.${fileExt}`;
        showFile(generatedFile?.payload, fileName);
      } else {
        throw Error('Failed to generate file');
      }
    }
  };

  return generateForm8;
};
