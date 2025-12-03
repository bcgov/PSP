import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

export interface IGenerateFormViewProps {
  isLoading: boolean;
}

const GenerateFormView: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormViewProps>
> = ({ children, isLoading }) => {
  const { hasClaim } = useKeycloakWrapper();

  return (
    <>
      <LoadingBackdrop show={isLoading} />
      {hasClaim(Claims.FORM_ADD) && (
        <>
          <StyledMenuGenerateWrapper>
            <StyledMenuHeaderWrapper>
              <StyledMenuHeader>Generate a form:</StyledMenuHeader>
            </StyledMenuHeaderWrapper>
            {children}
          </StyledMenuGenerateWrapper>
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
