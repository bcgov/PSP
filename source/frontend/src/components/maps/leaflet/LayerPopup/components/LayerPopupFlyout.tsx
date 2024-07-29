import React from 'react';
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
    <StyledFlyerWrapper>
      <StyledLinkSection>
        <StyledLinkWrapper>
          <LinkButton onClick={onViewPropertyInfo}>View Property Info</LinkButton>
        </StyledLinkWrapper>
      </StyledLinkSection>
      {!isRetiredProperty && (
        <StyledLinkSection>
          <StyledLinkWrapper>
            <StyledSubheading>Create:</StyledSubheading>
          </StyledLinkWrapper>
          {keycloak.hasClaim(Claims.RESEARCH_ADD) && (
            <StyledLinkWrapper>
              <LinkButton onClick={onCreateResearchFile}>Research File</LinkButton>
            </StyledLinkWrapper>
          )}
          {keycloak.hasClaim(Claims.ACQUISITION_ADD) && (
            <StyledLinkWrapper>
              <LinkButton onClick={onCreateAcquisitionFile}>Acquisition File</LinkButton>
            </StyledLinkWrapper>
          )}
          {keycloak.hasClaim(Claims.LEASE_ADD) && (
            <StyledLinkWrapper>
              <LinkButton onClick={onCreateLeaseLicense}>Lease/Licence File</LinkButton>
            </StyledLinkWrapper>
          )}
          {keycloak.hasClaim(Claims.DISPOSITION_ADD) && (
            <StyledLinkWrapper>
              <LinkButton onClick={onCreateDispositionFile}>Disposition File</LinkButton>
            </StyledLinkWrapper>
          )}
        </StyledLinkSection>
      )}

      {isRetiredProperty && (
        <StyledLinkSection>
          <StyledLinkWrapper>
            <StyledSubheading>Create:</StyledSubheading>
          </StyledLinkWrapper>
          {keycloak.hasClaim(Claims.RESEARCH_ADD) && (
            <StyledLinkWrapper>
              <LinkButton onClick={onCreateResearchFile}>Research File</LinkButton>
            </StyledLinkWrapper>
          )}
        </StyledLinkSection>
      )}

      {keycloak.hasClaim(Claims.PROPERTY_ADD) && isInPims && !isRetiredProperty && (
        <StyledLinkSection>
          <StyledLinkWrapper>
            <LinkButton onClick={onCreateSubdivision}>Create Subdivision</LinkButton>
          </StyledLinkWrapper>
          <StyledLinkWrapper>
            <LinkButton onClick={onCreateConsolidation}>Create Consolidation</LinkButton>
          </StyledLinkWrapper>
        </StyledLinkSection>
      )}
    </StyledFlyerWrapper>
  );
};

const StyledFlyerWrapper = styled.div`
  padding-left: 0.8rem;
`;

const StyledLinkSection = styled.div`
  border-bottom: 1px solid #bcbec5 !important;
  margin: 0rem 1rem 0rem 1rem !important;
  &:last-of-type {
    border-bottom: none !important;
    padding-bottom: 0.5rem !important;
  }
`;

const StyledSubheading = styled.div`
  padding-top: 0.5rem;
  font-weight: bold;
  font-size: 1.5rem;
`;

const StyledLinkWrapper = styled.div`
  .btn {
    width: 100%;
    font-size: 1.4rem !important;
  }
`;
