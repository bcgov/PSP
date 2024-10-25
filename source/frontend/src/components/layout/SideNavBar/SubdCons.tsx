import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import { HalfHeightDiv } from './styles';

/**
 * Subdivision & Consolidation Tray page.
 * Intended for use within the left side tray.
 */
export const SubdivisionConsolidationTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <HalfHeightDiv>
        {hasClaim(Claims.PROPERTY_ADD) && (
          <Link
            onClick={onLinkClick}
            to="/mapview/sidebar/subdivision/new"
            className="nav-item pl-9 pb-3"
          >
            Create a Subdivision
          </Link>
        )}
        {hasClaim(Claims.PROPERTY_ADD) && (
          <Link
            onClick={onLinkClick}
            to="/mapview/sidebar/consolidation/new"
            className="nav-item pl-9 pb-3"
          >
            Create a Consolidation
          </Link>
        )}
      </HalfHeightDiv>
    </>
  );
};
