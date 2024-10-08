import { MdTopic } from 'react-icons/md';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

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
        <Styled.TrayHeader>
          <span className="mr-2">
            <MdTopic size={26} />
          </span>
          Research Files
        </Styled.TrayHeader>
        {hasClaim(Claims.RESEARCH_VIEW) && (
          <Link onClick={onLinkClick} to="/research/list" className="nav-item">
            Manage Research Files
          </Link>
        )}
        {hasClaim(Claims.RESEARCH_ADD) && (
          <Link onClick={onLinkClick} to="/mapview/sidebar/research/new" className="nav-item">
            Create a Research File
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
