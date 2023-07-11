import { useCallback, useState } from 'react';

import { FormDocumentType } from '@/constants/formDocumentTypes';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useModalManagement } from '@/hooks/useModalManagement';
import { getMockApiCompensationList } from '@/mocks/compensations.mock';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

import { IGenerateFormViewProps } from './GenerateFormView';
import { useGenerateH120 } from './hooks/useGenerateH120';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateLetter } from './hooks/useGenerateLetter';
import { LetterRecipientModel } from './modals/models/LetterRecipientModel';

export interface IGenerateFormContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const GenerateFormContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [fullRecipientsList, setFullRecipientsList] = useState<LetterRecipientModel[]>([]);
  const { getPersonConcept, getOrganization } = useApiContacts();
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
  } = useAcquisitionProvider();
  const [isGenerateLetterModalOpened, openGenerateLetterModal, closeGenerateLetterModal] =
    useModalManagement();
  const {
    getAcquisitionOwners: {
      execute: retrieveAcquisitionFileOwners,
      loading: loadingAcquisitionFileOwners,
    },
  } = useAcquisitionProvider();
  const {
    getAcquisitionInterestHolders: {
      execute: fetchInterestHolders,
      loading: loadingInterestHolders,
    },
  } = useInterestHolderRepository();

  const fetchAllRecipients = useCallback(async () => {
    const generateRecipientsList: LetterRecipientModel[] = [];
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file === undefined) {
      return;
    }

    const fileOwnersFetchCall = retrieveAcquisitionFileOwners(acquisitionFileId);
    const interestHoldersFetchCall = fetchInterestHolders(acquisitionFileId);

    // Add owners and interest holders
    await Promise.all([fileOwnersFetchCall, interestHoldersFetchCall]).then(
      ([fileOwners, intHolders]) => {
        if (fileOwners !== undefined) {
          fileOwners?.map(owner =>
            generateRecipientsList.push(
              new LetterRecipientModel(owner.id!, 'OWNR', new Api_GenerateOwner(owner), owner),
            ),
          );
        }

        if (intHolders !== undefined) {
          intHolders.map(async holder => {
            if (holder.personId) {
              const person = (await getPersonConcept(holder?.personId))?.data;
              if (person) {
                generateRecipientsList.push(
                  new LetterRecipientModel(
                    holder.interestHolderId!,
                    'HLDR',
                    Api_GenerateOwner.fromApiPerson(person),
                    holder,
                  ),
                );
              }
            } else if (holder.organizationId) {
              const org = (await getOrganization(holder?.organizationId))?.data;
              if (org) {
                generateRecipientsList.push(
                  new LetterRecipientModel(
                    holder.interestHolderId!,
                    'HLDR',
                    Api_GenerateOwner.fromApiOrganization(org),
                    holder,
                  ),
                );
              }
            }
          });
        }
      },
    );

    // Add owners solicitors to recipients list
    if (file.acquisitionFileOwnerSolicitors && file.acquisitionFileOwnerSolicitors.length > 0) {
      await Promise.all(
        file.acquisitionFileOwnerSolicitors.map(async solicitor => {
          if (solicitor.personId) {
            const person = (await getPersonConcept(solicitor?.personId))?.data;
            if (person) {
              const personSolicitorModel = Api_GenerateOwner.fromApiPerson(person);
              generateRecipientsList.push(
                new LetterRecipientModel(person.id!, 'SLTR', personSolicitorModel, solicitor),
              );
            }
          } else if (solicitor.organizationId) {
            const org = (await getOrganization(solicitor?.organizationId))?.data;
            if (org) {
              const orgSolicitorModel = Api_GenerateOwner.fromApiOrganization(org);
              generateRecipientsList.push(
                new LetterRecipientModel(org.id!, 'SLTR', orgSolicitorModel, solicitor),
              );
            }
          }
        }),
      );
    }

    //Add Owner Representatives
    if (
      file.acquisitionFileOwnerRepresentatives &&
      file.acquisitionFileOwnerRepresentatives.length > 0
    ) {
      await Promise.all(
        file.acquisitionFileOwnerRepresentatives.map(async rep => {
          if (rep.personId) {
            const person = (await getPersonConcept(rep?.personId))?.data;
            if (person) {
              const personSolicitorModel = Api_GenerateOwner.fromApiPerson(person);
              generateRecipientsList.push(
                new LetterRecipientModel(rep.id!, 'REPT', personSolicitorModel, person),
              );
            }
          }
        }),
      );
    }

    setFullRecipientsList(generateRecipientsList);
    openGenerateLetterModal();
  }, [
    acquisitionFileId,
    fetchInterestHolders,
    getAcquisitionFile,
    getOrganization,
    getPersonConcept,
    openGenerateLetterModal,
    retrieveAcquisitionFileOwners,
  ]);

  const generateLetter = useGenerateLetter();
  const generateH0443 = useGenerateH0443();
  const generateH120 = useGenerateH120();
  const onGenerateClick = (formType: FormDocumentType) => {
    switch (formType) {
      case FormDocumentType.LETTER:
        fetchAllRecipients();
        break;
      case FormDocumentType.H0443:
        generateH0443(acquisitionFileId);
        break;
      case FormDocumentType.H120:
        generateH120(getMockApiCompensationList()[0]);
        break;
      default:
        console.error('Form Document type not recognized');
    }
  };

  const handleGenerateLetterCancel = (): void => {
    closeGenerateLetterModal();
  };

  const handleGenerateLetterOk = (recipients: Api_GenerateOwner[]): void => {
    generateLetter(acquisitionFileId, recipients);
    closeGenerateLetterModal();
  };

  return (
    <View
      onGenerateClick={onGenerateClick}
      isLoading={loadingAcquisitionFileOwners || loadingInterestHolders}
      letterRecipientsInitialValues={fullRecipientsList}
      openGenerateLetterModal={isGenerateLetterModalOpened}
      onGenerateLetterCancel={handleGenerateLetterCancel}
      onGenerateLetterOk={handleGenerateLetterOk}
    />
  );
};

export default GenerateFormContainer;
