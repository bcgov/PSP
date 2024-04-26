import { FormikProps } from 'formik/dist/types';
import { useRef } from 'react';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { FormDocumentType } from '@/constants/formDocumentTypes';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

import { generateDocumentEntries } from './formDocumentEntry';
import GenerateLetterRecipientsModal from './modals/GenerateLetterRecipientsModal';
import { LetterRecipientModel } from './modals/models/LetterRecipientModel';
import { LetterRecipientsForm } from './modals/models/LetterRecipientsForm';

export interface IGenerateFormViewProps {
  onGenerateClick: (formType: FormDocumentType) => void;
  isLoading: boolean;
  letterRecipientsInitialValues: LetterRecipientModel[];
  openGenerateLetterModal: boolean;
  onGenerateLetterCancel: () => void;
  onGenerateLetterOk: (recipients: Api_GenerateOwner[]) => void;
}

const GenerateFormView: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormViewProps>
> = ({
  onGenerateClick,
  openGenerateLetterModal,
  isLoading,
  letterRecipientsInitialValues,
  onGenerateLetterCancel,
  onGenerateLetterOk,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const entries = generateDocumentEntries;
  const formikRef = useRef<FormikProps<LetterRecipientsForm>>(null);

  return (
    <>
      {hasClaim(Claims.FORM_ADD) && (
        <>
          <LoadingBackdrop show={isLoading} />
          <StyledMenuGenerateWrapper>
            <StyledMenuHeaderWrapper>
              <StyledMenuHeader>Generate a form:</StyledMenuHeader>
            </StyledMenuHeaderWrapper>
            {entries.map(entry => (
              <LinkButton
                key={`generate-form-entry-${entry.formType}`}
                onClick={() => onGenerateClick(entry.formType)}
              >
                {entry.text}
              </LinkButton>
            ))}
          </StyledMenuGenerateWrapper>
          <GenerateLetterRecipientsModal
            isOpened={openGenerateLetterModal}
            recipientList={letterRecipientsInitialValues}
            onCancelClick={onGenerateLetterCancel}
            onGenerateLetterOk={onGenerateLetterOk}
            formikRef={formikRef}
          ></GenerateLetterRecipientsModal>
        </>
      )}
    </>
  );
};

export default GenerateFormView;

const StyledMenuWrapper = styled.div`
  text-align: left;
  padding: 0px;
  margin: 0px;
  width: 100%;
  color: ${props => props.theme.css.linkColor};
`;

const StyledMenuGenerateWrapper = styled(StyledMenuWrapper)`
  margin-top: auto;
  margin-bottom: 4rem;
`;

const StyledMenuHeaderWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  width: 100%;
  border-bottom: 1px solid ${props => props.theme.css.lightVariantColor};
`;

const StyledMenuHeader = styled.span`
  font-size: 1.4rem;
  color: ${props => props.theme.css.lightVariantColor};
  line-height: 2.2rem;
`;
