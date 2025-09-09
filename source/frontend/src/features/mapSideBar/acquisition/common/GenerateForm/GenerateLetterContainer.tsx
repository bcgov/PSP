import { FormikProps } from 'formik/dist/types';
import { useCallback, useMemo, useRef, useState } from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { InterestHolderType } from '@/constants/interestHolderTypes';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';
import { exists } from '@/utils';

import GenerateItemView from './GenerateItemView';
import GenerateLetterRecipientsModal from './modals/GenerateLetterRecipientsModal';
import { LetterRecipientModel, RecipientType } from './modals/models/LetterRecipientModel';
import { LetterRecipientsForm } from './modals/models/LetterRecipientsForm';

export interface IGenerateLetterContainerProps {
  acquisitionFileId: number;
  onGenerate: (recipients: Api_GenerateOwner[]) => void;
}

const GenerateLetterContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateLetterContainerProps>
> = ({ acquisitionFileId, onGenerate }) => {
  const [isGenerateLetterModalOpened, openGenerateLetterModal, closeGenerateLetterModal] =
    useModalManagement();

  const [fullRecipientsList, setFullRecipientsList] = useState<LetterRecipientModel[]>([]);

  const formikRef = useRef<FormikProps<LetterRecipientsForm>>(null);

  const { getPersonConcept, getOrganizationConcept } = useApiContacts();

  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
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
    const [fileOwners, intHolders] = await Promise.all([
      fileOwnersFetchCall,
      interestHoldersFetchCall,
    ]);

    if (exists(fileOwners)) {
      fileOwners.map(owner =>
        generateRecipientsList.push(
          new LetterRecipientModel(owner.id ?? 0, 'OWNR', new Api_GenerateOwner(owner), owner),
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
                  holder.interestHolderId ?? 0,
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
                  holder.interestHolderId ?? 0,
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
  }, [
    acquisitionFileId,
    fetchInterestHolders,
    getAcquisitionFile,
    getOrganizationConcept,
    getPersonConcept,
    retrieveAcquisitionFileOwners,
  ]);

  const handleGenerateClick = useCallback(() => {
    fetchAllRecipients();
    openGenerateLetterModal();
  }, [fetchAllRecipients, openGenerateLetterModal]);

  const handleGenerateLetterCancel = (): void => {
    closeGenerateLetterModal();
  };

  const handleGenerateLetterOk = (recipients: Api_GenerateOwner[]): void => {
    onGenerate(recipients);
    closeGenerateLetterModal();
  };

  const isLoading = useMemo(
    () => loadingAcquisitionFileOwners || loadingInterestHolders,
    [loadingAcquisitionFileOwners, loadingInterestHolders],
  );

  return (
    <>
      <LoadingBackdrop show={isLoading} />
      <GenerateItemView
        label="Generate Letter"
        formType={ApiGen_CodeTypes_FormTypes.LETTER}
        onGenerate={handleGenerateClick}
      />
      <GenerateLetterRecipientsModal
        isOpened={isGenerateLetterModalOpened}
        recipientList={fullRecipientsList}
        onCancelClick={handleGenerateLetterCancel}
        onGenerateLetterOk={handleGenerateLetterOk}
        formikRef={formikRef}
      />
    </>
  );
};

export default GenerateLetterContainer;

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
