import { Link } from 'react-router-dom';
import styled from 'styled-components';

import SubdivisionIcon from '@/assets/images/subdivision-icon.svg?react';
import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Subdivision & Consolidation Tray page.
 * Intended for use within the left side tray.
 */
export const SubdivisionConsolidationTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <HalfHeightDiv>
        <Styled.TrayHeader>
          <span className="mr-2">
            <SubdivisionIcon fill="currentColor" />
          </span>
          Subdivision & Consolidation
        </Styled.TrayHeader>
        {hasClaim(Claims.PROPERTY_ADD) && (
          <Link className="pl-9 pb-3" onClick={onLinkClick} to="/mapview/sidebar/subdivision/new">
            Create a Subdivision
          </Link>
        )}
        {hasClaim(Claims.PROPERTY_ADD) && (
          <Link className="pl-9 pb-3" onClick={onLinkClick} to="/mapview/sidebar/consolidation/new">
            Create a Consolidation
          </Link>
        )}
      </HalfHeightDiv>
    </>
  );
};

const HalfHeightDiv = styled.div`
  flex-direction: column;
  display: flex;
  height: 50%;
`;
