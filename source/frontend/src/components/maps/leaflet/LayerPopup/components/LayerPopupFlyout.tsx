import { LinkButton } from 'components/common/buttons';
import { Claims } from 'constants/claims';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import noop from 'lodash/noop';
import React from 'react';
import { ListGroup } from 'react-bootstrap';

export interface ILayerPopupFlyoutProps {
  pimsPropertyId: any;
  onViewPropertyInfo: () => void;
  onCreateResearchFile?: () => void;
  onCreateAcquisitionFile?: () => void;
  onCreateLeaseLicense?: () => void;
}

export const LayerPopupFlyout: React.FC<React.PropsWithChildren<ILayerPopupFlyoutProps>> = ({
  pimsPropertyId,
  onViewPropertyInfo,
  onCreateResearchFile,
  onCreateAcquisitionFile,
  onCreateLeaseLicense,
}) => {
  const keycloak = useKeycloakWrapper();
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
      {keycloak.hasClaim(Claims.LEASE_ADD) && (
        <ListGroup.Item>
          <LinkButton disabled={!pimsPropertyId} onClick={onCreateLeaseLicense ?? noop}>
            Lease/License - Create new
          </LinkButton>
        </ListGroup.Item>
      )}
    </ListGroup>
  );
};
