import { ContactMethodTypes } from 'constants/contactMethodType';
import { FormDocumentType } from 'constants/formDocumentTypes';
import { showFile } from 'features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { useProperties } from 'hooks/repositories/useProperties';
import { Api_AcquisitionFileOwner } from 'models/api/AcquisitionFile';
import { Api_Address } from 'models/api/Address';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { Api_Property } from 'models/api/Property';

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
        properties: properties !== undefined ? properties.map<Property>(x => getProperty(x)) : [],
        owner_names: owners.map<string>(x => getOwnerName(x)) || [],
        owner_contact: contactOwner !== undefined ? getOwnerContact(contactOwner) : null,
        property_coordinator: {
          first_name: coordinatorPerson?.firstName || '',
          last_name: coordinatorPerson?.surname || '',
        },
        property_agent: {
          first_name: agentPerson?.firstName || '',
          last_name: agentPerson?.surname || '',
          phone:
            agentPerson?.contactMethods?.find(
              x => x.contactMethodType?.id === ContactMethodTypes.WorkPhone,
            )?.value || '',
        },
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

function getOwnerContact(owner: Api_AcquisitionFileOwner): PersonWithPhone {
  if (owner.isOrganization) {
    return {
      last_name: owner.lastNameAndCorpName || '',
      first_name: owner.otherName || '',
      phone: owner.contactPhoneNum || '',
    };
  } else {
    return {
      last_name: owner.lastNameAndCorpName || '',
      first_name: owner.givenName || '',
      phone: owner.contactPhoneNum || '',
    };
  }
}

function getPropertyAddress(address: Api_Address): Address {
  return {
    municipality: address.municipality || '',
    postal_code: address.postal || '',
    province: address.province?.description || '',
    street_address_1: address.streetAddress1 || '',
    street_address_2: address.streetAddress2 || '',
    street_address_3: address.streetAddress3 || '',
  };
}

function getProperty(property: Api_Property): Property {
  return {
    address: property.address !== undefined ? getPropertyAddress(property.address) : null,
    legal_description: property.landLegalDescription || '',
    pid: property.pid?.toString() || '',
  };
}

interface Person {
  first_name: string;
  last_name: string;
}

interface PersonWithPhone extends Person {
  phone: string;
}

interface Address {
  municipality: string;
  postal_code: string;
  province: string;
  street_address_1: string;
  street_address_2: string;
  street_address_3: string;
}

interface Property {
  address: Address | null;
  legal_description: string;
  pid: string;
}

interface H0443Data {
  file_name: string;
  file_number: string;
  owner_contact: PersonWithPhone | null;
  owner_names: string[];
  project_number: string;
  project_name: string;
  properties: Property[];
  property_agent: PersonWithPhone;
  property_coordinator: Person;
}
