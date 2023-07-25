import React from 'react';
import { ListGroup } from 'react-bootstrap';

import { LinkButton } from '@/components/common/buttons';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

export interface ILayerPopupFlyoutProps {
  onViewPropertyInfo: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateResearchFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateAcquisitionFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateLeaseLicense: (event: React.MouseEvent<HTMLElement>) => void;
}

export const LayerPopupFlyout: React.FC<React.PropsWithChildren<ILayerPopupFlyoutProps>> = ({
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
          <LinkButton onClick={onCreateResearchFile}>Research File - Create new</LinkButton>
        </ListGroup.Item>
      )}
      {keycloak.hasClaim(Claims.ACQUISITION_ADD) && (
        <ListGroup.Item>
          <LinkButton onClick={onCreateAcquisitionFile}>Acquisition File - Create new</LinkButton>
        </ListGroup.Item>
      )}
      {keycloak.hasClaim(Claims.LEASE_ADD) && (
        <ListGroup.Item>
          <LinkButton onClick={onCreateLeaseLicense}>Lease/License - Create new</LinkButton>
        </ListGroup.Item>
      )}
    </ListGroup>
  );
};
