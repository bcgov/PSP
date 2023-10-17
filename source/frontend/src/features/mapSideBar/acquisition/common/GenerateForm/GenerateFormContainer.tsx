import { useCallback, useState } from 'react';

import { FormDocumentType } from '@/constants/formDocumentTypes';
import { InterestHolderType } from '@/constants/interestHolderTypes';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useModalManagement } from '@/hooks/useModalManagement';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

import { IGenerateFormViewProps } from './GenerateFormView';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateLetter } from './hooks/useGenerateLetter';
import { LetterRecipientModel, RecipientType } from './modals/models/LetterRecipientModel';

export interface IGenerateFormContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const GenerateFormContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [fullRecipientsList, setFullRecipientsList] = useState<LetterRecipientModel[]>([]);
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
    getAcquisitionOwners: {
      execute: retrieveAcquisitionFileOwners,
      loading: loadingAcquisitionFileOwners,
    },
  } = useAcquisitionProvider();
  const [isGenerateLetterModalOpened, openGenerateLetterModal, closeGenerateLetterModal] =
    useModalManagement();
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
    const [fileOwners, intHolders] = await Promise.all([
      fileOwnersFetchCall,
      interestHoldersFetchCall,
    ]);

    if (fileOwners) {
      fileOwners?.map(owner =>
        generateRecipientsList.push(
          new LetterRecipientModel(owner.id!, 'OWNR', new Api_GenerateOwner(owner), owner),
        ),
      );
    }

    if (intHolders) {
      await Promise.all(
        intHolders.map(async holder => {
          if (holder.personId) {
            const person = (await getPersonConcept(holder?.personId))?.data;
            if (person) {
              generateRecipientsList.push(
                new LetterRecipientModel(
                  holder.interestHolderId!,
                  getInterestTypeString(holder.interestHolderType?.id ?? ''),
                  Api_GenerateOwner.fromApiPerson(person),
                  holder,
                ),
              );
            }
          } else if (holder.organizationId) {
            const org = (await getOrganizationConcept(holder?.organizationId))?.data;
            if (org) {
              generateRecipientsList.push(
                new LetterRecipientModel(
                  holder.interestHolderId!,
                  getInterestTypeString(holder.interestHolderType?.id ?? ''),
                  Api_GenerateOwner.fromApiOrganization(org),
                  holder,
                ),
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
    getOrganizationConcept,
    getPersonConcept,
    openGenerateLetterModal,
    retrieveAcquisitionFileOwners,
  ]);

  const generateLetter = useGenerateLetter();
  const generateH0443 = useGenerateH0443();
  const onGenerateClick = (formType: FormDocumentType) => {
    switch (formType) {
      case FormDocumentType.LETTER:
        fetchAllRecipients();
        break;
      case FormDocumentType.H0443:
        generateH0443(acquisitionFileId);
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

const getInterestTypeString = (codeType: string): RecipientType => {
  let interestString: RecipientType = 'HLDR';
  switch (codeType) {
    case InterestHolderType.INTEREST_HOLDER:
      interestString = 'HLDR';
      break;
    case InterestHolderType.OWNER_SOLICITOR:
      interestString = 'SLTR';
      break;
    case InterestHolderType.OWNER_REPRESENTATIVE:
      interestString = 'REPT';
      break;
    default:
      interestString = 'OWNR';
      break;
  }

  return interestString;
};
