import * as React from 'react';
import { useCallback, useState } from 'react';
import Nav from 'react-bootstrap/Nav';
import { FaQuestionCircle } from 'react-icons/fa';
import styled from 'styled-components';

import TooltipWrapper from '@/components/common/TooltipWrapper';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import HelpModal from '../components/HelpModal';

/**
 * A help icon that displays the Help Modal when clicked. Does not display unless the user is authenticated.
 * @param props
 */
export function HelpContainer() {
  const [showHelp, setShowHelp] = useState(false);
  const keycloak = useKeycloakWrapper();

  const handleCancel = useCallback(() => setShowHelp(false), []);

  return keycloak.obj.authenticated ? (
    <Nav.Item>
      <TooltipWrapper toolTipId="help-tooltip" toolTip="Ask for Help">
        <StyledHelpIcon onClick={() => setShowHelp(true)} />
      </TooltipWrapper>
      <HelpModal
        show={showHelp}
        handleCancel={handleCancel}
        handleSubmit={handleCancel}
      ></HelpModal>
    </Nav.Item>
  ) : null;
}

const StyledHelpIcon = styled(FaQuestionCircle)`
  cursor: pointer;
`;

export default HelpContainer;
