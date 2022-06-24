import { LinkButton } from 'components/common/buttons';
import { Claims } from 'constants/claims';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import noop from 'lodash/noop';
import React from 'react';
import { ListGroup } from 'react-bootstrap';

export interface ILayerPopupFlyoutProps {
  onViewPropertyInfo: () => void;
  onCreateResearchFile?: () => void;
  onCreateAcquisitionFile: () => void;
}

export const LayerPopupFlyout: React.FC<ILayerPopupFlyoutProps> = ({
  onViewPropertyInfo,
  onCreateResearchFile,
  onCreateAcquisitionFile,
}) => {
  const keycloak = useKeycloakWrapper();
  console.log(keycloak.obj);
  return (
    <ListGroup variant="flush">
      <ListGroup.Item>
        <LinkButton onClick={onViewPropertyInfo}>View more property info</LinkButton>
      </ListGroup.Item>
      {keycloak.hasClaim(Claims.RESEARCH_ADD) && (
        <ListGroup.Item>
          <LinkButton onClick={onCreateResearchFile ?? noop}>Research File - Create new</LinkButton>
        </ListGroup.Item>
      )}
      {keycloak.hasClaim(Claims.ACQUISITION_ADD) && (
        <ListGroup.Item>
          <LinkButton onClick={onCreateAcquisitionFile ?? noop}>
            Acquisition File - Create new
          </LinkButton>
        </ListGroup.Item>
      )}
    </ListGroup>
  );
};
