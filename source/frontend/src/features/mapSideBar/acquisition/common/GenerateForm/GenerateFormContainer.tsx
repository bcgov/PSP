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
    let i = 0;
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
          const ownersLetterModel = fileOwners?.map(owner => new Api_GenerateOwner(owner)) ?? [];
          const ownerRecipientsLetterModel =
            ownersLetterModel.map(x => new LetterRecipientModel(i++, 'OWNR', x)) ?? [];
          generateRecipientsList.push(...ownerRecipientsLetterModel);
        }

        if (intHolders !== undefined) {
          const intHolderGenLetterModel: LetterRecipientModel[] = intHolders.map(
            x => new LetterRecipientModel(i++, 'HLDR', Api_GenerateOwner.fromInterestHolder(x), x),
          );

          generateRecipientsList.push(...intHolderGenLetterModel);
        }
      },
    );

    // Add owners solicitors to recipients list
    if (file.acquisitionFileOwnerSolicitors && file.acquisitionFileOwnerSolicitors.length > 0) {
      await Promise.all(
        file.acquisitionFileOwnerSolicitors.map(async solicitor => {
          if (solicitor.personId) {
            const person = !!solicitor?.personId
              ? (await getPersonConcept(solicitor?.personId))?.data
              : null;

            if (person) {
              const personSolicitorModel = Api_GenerateOwner.fromApiPerson(person);
              generateRecipientsList.push(
                new LetterRecipientModel(i++, 'SLTR', personSolicitorModel, solicitor),
              );
            }
          } else if (solicitor.organizationId) {
            const org = !!solicitor?.organizationId
              ? (await getOrganization(solicitor?.organizationId))?.data
              : null;
            if (org) {
              const orgSolicitorModel = Api_GenerateOwner.fromOrganization(org);
              generateRecipientsList.push(
                new LetterRecipientModel(i++, 'SLTR', orgSolicitorModel, solicitor),
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
            const person = !!rep?.personId ? (await getPersonConcept(rep?.personId))?.data : null;
            if (person) {
              const personSolicitorModel = Api_GenerateOwner.fromApiPerson(person);
              generateRecipientsList.push(
                new LetterRecipientModel(i++, 'REPT', personSolicitorModel, person),
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
