import { FormDocumentType } from '@/constants/formDocumentTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProperties } from '@/hooks/repositories/useProperties';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';
import { Api_GeneratePerson } from '@/models/generate/GeneratePerson';
import { Api_GenerateProperty } from '@/models/generate/GenerateProperty';
import { exists, isValidId } from '@/utils';

export const useGenerateH0443 = () => {
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
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
      // Retrieve Property Coordinator
      const propertyCoordinator = file.acquisitionTeam?.find(
        team => team.teamProfileTypeCode === 'PROPCOORD',
      );

      // Retrieve Property Agent
      const propertyAgent = file.acquisitionTeam?.find(
        team => team.teamProfileTypeCode === 'PROPAGENT',
      );

      // Retrieve Properties
      const filePropertiesIds =
        file.fileProperties?.map(fp => fp.propertyId).filter(isValidId) || [];
      const properties = await getMultipleProperties(filePropertiesIds);

      const owners: ApiGen_Concepts_AcquisitionFileOwner[] =
        file.acquisitionFileOwners?.filter(exists) || [];
      const contactOwner = owners.find(x => x.isPrimaryContact === true);

      const h0443Data: H0443Data = {
        file_name: file.fileName || '',
        file_number: file.fileNumber || '',
        project_number: file.project?.code || '',
        project_name: file.project?.description || '',
        properties:
          properties !== undefined
            ? properties.map<Api_GenerateProperty>(x => new Api_GenerateProperty(x))
            : [],
        owner_names: owners.map<string>(x => getOwnerName(x)) || [],
        owner_contact: contactOwner !== undefined ? new Api_GenerateOwner(contactOwner) : null,
        property_coordinator: null,
        property_agent: null,
      };

      // Get the property coordinator by checking if it's a person or an org.
      if (propertyCoordinator) {
        if (propertyCoordinator.personId) {
          const personConceptResponse = await getPersonConcept(propertyCoordinator?.personId);

          h0443Data.property_coordinator = new Api_GeneratePerson(personConceptResponse.data);
        } else if (propertyCoordinator.organizationId) {
          const organizationConceptResponse = await getOrganizationConcept(
            propertyCoordinator?.organizationId,
          );

          if (
            organizationConceptResponse &&
            organizationConceptResponse?.data &&
            organizationConceptResponse?.data.organizationPersons?.length &&
            propertyCoordinator.primaryContactId
          ) {
            const personResponse = await getPersonConcept(propertyCoordinator.primaryContactId);

            h0443Data.property_coordinator = new Api_GeneratePerson(personResponse.data);
          }
        }
      }

      // Get the property agent by checking if it's a person or an org.
      if (propertyAgent) {
        if (propertyAgent.personId) {
          const personResponse = await getPersonConcept(propertyAgent?.personId);

          h0443Data.property_agent = new Api_GeneratePerson(personResponse.data);
        } else if (propertyAgent.organizationId) {
          const organizationConceptResponse = await getOrganizationConcept(
            propertyAgent?.organizationId,
          );

          if (
            organizationConceptResponse.data &&
            organizationConceptResponse?.data.organizationPersons?.length &&
            propertyAgent.primaryContactId
          ) {
            const personResponse = await getPersonConcept(propertyAgent.primaryContactId);

            h0443Data.property_agent = new Api_GeneratePerson(personResponse.data);
          }
        }
      }

      const generatedFile = await generate({
        templateType: FormDocumentType.H0443,
        templateData: h0443Data,
        convertToType: null,
      });
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
        generatedFile?.payload &&
        showFile(generatedFile?.payload);
    }
  };
  return generateLetter;
};

function getOwnerName(owner: ApiGen_Concepts_AcquisitionFileOwner): string {
  if (owner.isOrganization) {
    let corpName: string = owner.lastNameAndCorpName || '';
    if (owner.incorporationNumber) {
      corpName += ` (Inc. No. ${owner.incorporationNumber})`;
    } else {
      corpName += ` (Reg. No. ${owner.registrationNumber})`;
    }
    return corpName;
  } else {
    const personName = `${owner.givenName} ${owner.lastNameAndCorpName}`;
    return personName;
  }
}

interface H0443Data {
  file_name: string;
  file_number: string;
  owner_contact: Api_GenerateOwner | null;
  owner_names: string[];
  project_number: string;
  project_name: string;
  properties: Api_GenerateProperty[];
  property_agent: Api_GeneratePerson | null;
  property_coordinator: Api_GeneratePerson | null;
}
