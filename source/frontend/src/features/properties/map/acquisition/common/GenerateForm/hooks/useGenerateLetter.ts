import { FormDocumentType } from 'constants/formDocumentTypes';
import { showFile } from 'features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { Api_GenerateLetter } from 'models/generate/GenerateLetter';

export const useGenerateLetter = () => {
  const { getPersonConcept } = useApiContacts();
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
  } = useAcquisitionProvider();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateLetter = async (acquisitionFileId: number) => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file) {
      var a = file;
      console.log(a, typeof a);
      const coordinator = file.acquisitionTeam?.find(
        team => team.personProfileTypeCode === 'PROPCOORD',
      );
      console.log('coordinator', coordinator);
      const coordinatorPerson = !!coordinator?.personId
        ? (await getPersonConcept(coordinator?.personId))?.data
        : null;
      const letterData = new Api_GenerateLetter(file, coordinatorPerson);
      const generatedFile = await generate({
        templateType: FormDocumentType.LETTER,
        templateData: letterData,
      });
      generatedFile?.status === ExternalResultStatus.Success!! &&
        generatedFile?.payload &&
        showFile(generatedFile?.payload);
    }
  };
  return generateLetter;
};
