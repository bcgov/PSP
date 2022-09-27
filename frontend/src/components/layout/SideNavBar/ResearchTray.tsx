import { Claims } from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Research Tray page.
 * Intended for use within the left side tray.
 */
export const ResearchTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <HalfHeightDiv>
        <Styled.TrayHeader>Research Files</Styled.TrayHeader>
        {hasClaim(Claims.RESEARCH_VIEW) && (
          <Link onClick={onLinkClick} to="/research/list">
            Manage Research files
          </Link>
        )}
        {hasClaim(Claims.RESEARCH_ADD) && (
          <Link onClick={onLinkClick} to="/mapview/sidebar/research/new">
            Create a Research file
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
