import { useCallback, useState } from 'react';
import Nav from 'react-bootstrap/Nav';
import { FaQuestionCircle } from 'react-icons/fa';
import styled from 'styled-components';

import TooltipWrapper from '@/components/common/TooltipWrapper';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useTenant } from '@/tenants/useTenant';

import HelpModal from '../components/HelpModal';

/**
 * A help icon that displays the Help Modal when clicked. Does not display unless the user is authenticated.
 * @param props
 */
export function HelpContainer() {
  const [showHelp, setShowHelp] = useState(false);
  const keycloak = useKeycloakWrapper();

  const handleCancel = useCallback(() => setShowHelp(false), []);
  const tenant = useTenant();

  return keycloak.obj.authenticated ? (
    <Nav.Item>
      <TooltipWrapper toolTipId="help-tooltip" toolTip="Ask for Help">
        <StyledContainer onClick={() => setShowHelp(true)}>
          <StyledHelpIcon />
          <label>Help</label>
        </StyledContainer>
      </TooltipWrapper>
      <HelpModal
        show={showHelp}
        handleCancel={handleCancel}
        handleSubmit={handleCancel}
        pimsTrainingUrl={tenant.pimsTrainingResourceUrl}
      ></HelpModal>
    </Nav.Item>
  ) : null;
}

const StyledHelpIcon = styled(FaQuestionCircle)`
  cursor: pointer;
`;

const StyledContainer = styled.div`
  display: flex;
  align-items: center;
  flex-direction: row;
  label {
    margin-left: 1rem;
    margin-bottom: 0;
    &:hover {
      cursor: pointer;
    }
  }
`;

export default HelpContainer;
