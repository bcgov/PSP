import React from 'react';
import { ListGroup } from 'react-bootstrap';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

export interface ILayerPopupFlyoutProps {
  isInPims: boolean;
  isRetiredProperty: boolean;
  onViewPropertyInfo: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateResearchFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateAcquisitionFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateLeaseLicense: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateDispositionFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateSubdivision: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateConsolidation: (event: React.MouseEvent<HTMLElement>) => void;
}

export const LayerPopupFlyout: React.FC<React.PropsWithChildren<ILayerPopupFlyoutProps>> = ({
  isInPims,
  isRetiredProperty,
  onViewPropertyInfo,
  onCreateResearchFile,
  onCreateAcquisitionFile,
  onCreateLeaseLicense,
  onCreateDispositionFile,
  onCreateSubdivision,
  onCreateConsolidation,
}) => {
  const keycloak = useKeycloakWrapper();

  return (
    <ListGroup variant="flush">
      <StyledLinkSection>
        <ListGroup.Item>
          <LinkButton onClick={onViewPropertyInfo}>View Property info</LinkButton>
        </ListGroup.Item>
      </StyledLinkSection>
      {!isRetiredProperty && (
        <StyledLinkSection>
          <ListGroup.Item>
            <StyledSubheading>Create:</StyledSubheading>
          </ListGroup.Item>
          {keycloak.hasClaim(Claims.RESEARCH_ADD) && (
            <ListGroup.Item>
              <LinkButton onClick={onCreateResearchFile}>Research File</LinkButton>
            </ListGroup.Item>
          )}
          {keycloak.hasClaim(Claims.ACQUISITION_ADD) && (
            <ListGroup.Item>
              <LinkButton onClick={onCreateAcquisitionFile}>Acquisition File</LinkButton>
            </ListGroup.Item>
          )}
          {keycloak.hasClaim(Claims.LEASE_ADD) && (
            <ListGroup.Item>
              <LinkButton onClick={onCreateLeaseLicense}>Lease/Licence</LinkButton>
            </ListGroup.Item>
          )}
          {keycloak.hasClaim(Claims.DISPOSITION_ADD) && (
            <ListGroup.Item>
              <LinkButton onClick={onCreateDispositionFile}>Disposition File</LinkButton>
            </ListGroup.Item>
          )}
        </StyledLinkSection>
      )}

      {isRetiredProperty && (
        <StyledLinkSection>
          <ListGroup.Item>
            <StyledSubheading>Create:</StyledSubheading>
          </ListGroup.Item>
          {keycloak.hasClaim(Claims.RESEARCH_ADD) && (
            <ListGroup.Item>
              <LinkButton onClick={onCreateResearchFile}>Research File</LinkButton>
            </ListGroup.Item>
          )}
        </StyledLinkSection>
      )}

      {keycloak.hasClaim(Claims.PROPERTY_ADD) && isInPims && !isRetiredProperty && (
        <StyledLinkSection>
          <ListGroup.Item>
            <LinkButton onClick={onCreateSubdivision}>Create Subdivision</LinkButton>
          </ListGroup.Item>
          <ListGroup.Item>
            <LinkButton onClick={onCreateConsolidation}>Create Consolidation</LinkButton>
          </ListGroup.Item>
        </StyledLinkSection>
      )}
    </ListGroup>
  );
};

const StyledLinkSection = styled.span`
  border-bottom: 1px solid #bcbec5 !important;
  margin: 0rem 1rem 0rem 1rem !important;
  &:last-of-type {
    border-bottom: none !important;
    padding-bottom: 0.5rem !important;
  }

  .list-group-item {
    padding: 0.5rem 1rem 0 0rem !important;
    .btn {
      width: 100%;
    }
  }
`;

const StyledSubheading = styled.div`
  padding-top: 0.5rem;
  font-weight: bold;
  font-size: 16px;
`;
