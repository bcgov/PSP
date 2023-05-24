import { FormDocumentType } from 'constants/formDocumentTypes';
import { showFile } from 'features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { useProperties } from 'hooks/repositories/useProperties';
import { Api_AcquisitionFileOwner } from 'models/api/AcquisitionFile';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { GenerateOwner } from 'models/generate/GenerateOwner';
import { GeneratePerson } from 'models/generate/GeneratePerson';
import { GenerateProperty } from 'models/generate/GenerateProperty';

export const useGenerateH0443 = () => {
  const { getPersonConcept } = useApiContacts();
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
  } = useAcquisitionProvider();

  const {
    getMultiplePropertiesById: { execute: getMultipleProperties },
  } = useProperties();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateLetter = async (acquisitionFileId: number) => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file) {
      // Retrieve Propery Coordinator
      const propertyCoordinator = file.acquisitionTeam?.find(
        team => team.personProfileTypeCode === 'PROPCOORD',
      );
      const coordinatorPerson = !!propertyCoordinator?.personId
        ? (await getPersonConcept(propertyCoordinator?.personId))?.data
        : null;

      // Retrieve Property Agent
      const propertyAgent = file.acquisitionTeam?.find(
        team => team.personProfileTypeCode === 'PROPAGENT',
      );

      const agentPerson = !!propertyAgent?.personId
        ? (await getPersonConcept(propertyAgent?.personId))?.data
        : null;

      // Retrieve Properties
      const filePropertiesIds =
        file.fileProperties?.map(fp => fp.propertyId).filter((p): p is number => !!p) || [];
      const properties = await getMultipleProperties(filePropertiesIds);

      const owners: Api_AcquisitionFileOwner[] =
        file.acquisitionFileOwners?.filter((x): x is Api_AcquisitionFileOwner => !!x) || [];
      const contactOwner = owners.find(x => x.isPrimaryContact === true);

      const h0443Data: H0443Data = {
        file_name: file.fileName || '',
        file_number: file.fileNumber || '',
        project_number: file.project?.code || '',
        project_name: file.project?.description || '',
        properties:
          properties !== undefined
            ? properties.map<GenerateProperty>(x => new GenerateProperty(x))
            : [],
        owner_names: owners.map<string>(x => getOwnerName(x)) || [],
        owner_contact: contactOwner !== undefined ? new GenerateOwner(contactOwner) : null,
        property_coordinator: new GeneratePerson(coordinatorPerson),
        property_agent: new GeneratePerson(agentPerson),
      };

      const generatedFile = await generate({
        templateType: FormDocumentType.H0443,
        templateData: h0443Data,
      });
      generatedFile?.status === ExternalResultStatus.Success!! &&
        generatedFile?.payload &&
        showFile(generatedFile?.payload);
    }
  };
  return generateLetter;
};

function getOwnerName(owner: Api_AcquisitionFileOwner): string {
  if (owner.isOrganization) {
    var corpName: string = owner.lastNameAndCorpName || '';
    if (owner.incorporationNumber) {
      corpName += ` (Inc. No. ${owner.incorporationNumber})`;
    } else {
      corpName += ` (Reg. No. ${owner.registrationNumber})`;
    }
    return corpName;
  } else {
    var personName: string = `${owner.givenName} ${owner.lastNameAndCorpName}`;
    return personName;
  }
}

interface H0443Data {
  file_name: string;
  file_number: string;
  owner_contact: GenerateOwner | null;
  owner_names: string[];
  project_number: string;
  project_name: string;
  properties: GenerateProperty[];
  property_agent: GeneratePerson;
  property_coordinator: GeneratePerson;
}
