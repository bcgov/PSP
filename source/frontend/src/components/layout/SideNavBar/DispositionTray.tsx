import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Disposition Tray page.
 * Intended for use within the left side tray.
 */
export const DispositionTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <HalfHeightDiv>
        <Styled.TrayHeader>Disposition Files</Styled.TrayHeader>
        {hasClaim(Claims.RESEARCH_VIEW /*TODO: DISPOSITION_VIEW*/) && (
          <Link onClick={onLinkClick} to="/disposition/list">
            Manage Disposition Files
          </Link>
        )}
        {hasClaim(Claims.RESEARCH_ADD /*TODO: DISPOSITION_ADD*/) && (
          <Link onClick={onLinkClick} to="/mapview/sidebar/disposition/new">
            Create a Disposition File
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
