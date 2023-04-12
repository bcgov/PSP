import { showFile } from 'features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { ExternalResultStatus } from 'models/api/ExternalResult';

import { Api_AcquisitionFile } from './../models/api/AcquisitionFile';
import { Api_GenerateLetter } from './../models/generate/GenerateLetter';
import { useApiContacts } from './pims-api/useApiContacts';
export const useGenerateLetter = () => {
  const { getPersonConcept } = useApiContacts();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const generateLetter = async (file: Api_AcquisitionFile) => {
    const coordinator = file.acquisitionTeam?.find(
      team => team.personProfileTypeCode === 'PROPCOORD',
    );
    const coordinatorPerson = !!coordinator?.personId
      ? (await getPersonConcept(coordinator?.personId))?.data
      : null;
    const letterData = new Api_GenerateLetter(file, coordinatorPerson);
    const generatedFile = await generate({ templateType: 'LETTER', templateData: letterData });
    generatedFile?.status === ExternalResultStatus.Success!! &&
      generatedFile?.payload &&
      showFile(generatedFile?.payload);
  };
  return generateLetter;
};
