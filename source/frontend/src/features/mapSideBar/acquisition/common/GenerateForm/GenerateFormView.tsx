import { FormikProps } from 'formik/dist/types';
import { useRef } from 'react';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { FormDocumentType } from '@/constants/formDocumentTypes';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

import { FormDocumentEntry } from './formDocumentEntry';
import GenerateLetterRecipientsModal from './modals/GenerateLetterRecipientsModal';
import { LetterRecipientModel } from './modals/models/LetterRecipientModel';
import { LetterRecipientsForm } from './modals/models/LetterRecipientsForm';

export interface IGenerateFormViewProps {
  formEntries: FormDocumentEntry[];
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
  formEntries,
  onGenerateClick,
  openGenerateLetterModal,
  isLoading,
  letterRecipientsInitialValues,
  onGenerateLetterCancel,
  onGenerateLetterOk,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const formikRef = useRef<FormikProps<LetterRecipientsForm>>(null);

  if (formEntries.length === 0) {
    return <></>;
  }

  return (
    <>
      {hasClaim(Claims.FORM_ADD) && (
        <>
          <LoadingBackdrop show={isLoading} />
          <StyledMenuGenerateWrapper>
            <StyledMenuHeaderWrapper>
              <StyledMenuHeader>Generate a form:</StyledMenuHeader>
            </StyledMenuHeaderWrapper>
            {formEntries.map(entry => (
              <LinkButton
                key={`generate-form-entry-${entry.formType}`}
                onClick={() => onGenerateClick(entry.formType)}
                title="Download File"
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
          />
        </>
      )}
    </>
  );
};

export default GenerateFormView;

const StyledMenuGenerateWrapper = styled.div`
  margin-bottom: 4rem;
  width: 100%;
  color: ${props => props.theme.css.linkColor};
  text-align: left;
`;

const StyledMenuHeaderWrapper = styled.div`
  justify-content: space-between;
  width: 100%;
  border-bottom: 1px solid ${props => props.theme.css.borderOutlineColor};
`;

const StyledMenuHeader = styled.span`
  font-size: 1.4rem;
  color: ${props => props.theme.css.themeGray70};
  line-height: 2.2rem;
`;
