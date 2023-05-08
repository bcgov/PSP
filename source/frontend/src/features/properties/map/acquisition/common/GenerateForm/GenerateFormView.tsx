import { LinkButton } from 'components/common/buttons';
import { FormDocumentType } from 'constants/formDocumentTypes';
import { Claims } from 'constants/index';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import styled from 'styled-components';

import { generateDocumentEntries } from './formDocumentEntry';

export interface IGenerateFormViewProps {
  onGenerateClick: (formType: FormDocumentType) => void;
}

const GenerateFormView: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormViewProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const entries = generateDocumentEntries;
  return (
    <>
      {hasClaim(Claims.FORM_ADD) && (
        <StyledMenuGenerateWrapper>
          <StyledMenuHeaderWrapper>
            <StyledMenuHeader>Generate a form:</StyledMenuHeader>
          </StyledMenuHeaderWrapper>
          {entries.map(entry => (
            <LinkButton
              key={`generate-form-entry-${entry.formType}`}
              onClick={() => props.onGenerateClick(entry.formType)}
            >
              {entry.text}
            </LinkButton>
          ))}
        </StyledMenuGenerateWrapper>
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
