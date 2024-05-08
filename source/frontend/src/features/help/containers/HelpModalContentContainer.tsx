import { useLocation } from 'react-router-dom';
import styled from 'styled-components';

import { H3 } from '@/components/common/styles';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import HelpSubmitBox from '../components/HelpSubmitBox';

interface IHelpModalContentContainerProps {
  /** Set the content of the parent mailto component based on the ticket form. */
  setMailto: (mailto: { subject: string; body: string; email: string }) => void;
}

/**
 * Provides logic for modal content. User information is provided by keycloak. The current page is determined using the react router location.
 */
const HelpModalContentContainer: React.FunctionComponent<
  React.PropsWithChildren<IHelpModalContentContainerProps>
> = ({ setMailto }) => {
  const keycloak = useKeycloakWrapper();
  const location = useLocation();
  const displayName = keycloak.displayName;
  const email = keycloak.email;
  return (
    <>
      <H3Styled>Contact us:</H3Styled>
      <HelpSubmitBox
        user={displayName ?? ''}
        email={email ?? ''}
        page={location.pathname}
        setMailto={setMailto}
      />
    </>
  );
};

const H3Styled = styled(H3)`
  border: none;
  margin-bottom: 16px;
  margin-top: 24px;
`;

export default HelpModalContentContainer;
