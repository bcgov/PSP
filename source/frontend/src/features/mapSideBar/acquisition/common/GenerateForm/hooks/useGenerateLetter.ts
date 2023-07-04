import { FormDocumentType } from '@/constants/formDocumentTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ExternalResultStatus } from '@/models/api/ExternalResult';
import { Api_GenerateLetter } from '@/models/generate/GenerateLetter';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

export const useGenerateLetter = () => {
  const { getPersonConcept } = useApiContacts();
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
  } = useAcquisitionProvider();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateLetter = async (
    acquisitionFileId: number,
    recipients: Api_GenerateOwner[] | null = null,
  ) => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file) {
      const coordinator = file.acquisitionTeam?.find(
        team => team.personProfileTypeCode === 'PROPCOORD',
      );
      const coordinatorPerson = !!coordinator?.personId
        ? (await getPersonConcept(coordinator?.personId))?.data
        : null;
      const letterData = new Api_GenerateLetter(file, coordinatorPerson);
      letterData.owners = recipients ?? letterData.owners;
      const generatedFile = await generate({
        templateType: FormDocumentType.LETTER,
        templateData: letterData,
        convertToType: null,
      });
      generatedFile?.status === ExternalResultStatus.Success!! &&
        generatedFile?.payload &&
        showFile(generatedFile?.payload);
    }
  };

  return generateLetter;
};
