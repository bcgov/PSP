import { FormDocumentType } from '@/constants/formDocumentTypes';
import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { Api_GenerateLetter } from '@/models/generate/GenerateLetter';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';
import { isValidId } from '@/utils';

export const useGenerateLetter = () => {
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
    getAcquisitionProperties: { execute: getAcquisitionProperties },
  } = useAcquisitionProvider();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateLetter = async (
    acquisitionFileId: number,
    recipients: Api_GenerateOwner[] | null = null,
  ) => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file) {
      const properties = await getAcquisitionProperties(acquisitionFileId);
      file.fileProperties = properties ?? [];
      const coordinator = file.acquisitionTeam?.find(
        team => team.teamProfileTypeCode === 'PROPCOORD',
      );
      if (isValidId(coordinator?.personId)) {
        coordinator!.person = (await getPersonConcept(coordinator!.personId))?.data;
      } else if (isValidId(coordinator?.organizationId)) {
        coordinator!.organization = (
          await getOrganizationConcept(coordinator!.organizationId)
        )?.data;
      }
      const letterData = new Api_GenerateLetter(file, coordinator);
      letterData.owners = recipients ?? letterData.owners;
      const generatedFile = await generate({
        templateType: FormDocumentType.LETTER,
        templateData: letterData,
        convertToType: null,
      });
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
        generatedFile?.payload &&
        createFileDownload(generatedFile?.payload);
    }
  };

  return generateLetter;
};
