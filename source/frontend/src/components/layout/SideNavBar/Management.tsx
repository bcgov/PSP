import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import { HalfHeightDiv } from './styles';

/**
 * Management Tray page with links to lis management functionality.
 * Intended for use within the left side tray.
 */
export const Management = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <HalfHeightDiv>
      {hasClaim(Claims.MANAGEMENT_VIEW) && (
        <>
          <Link onClick={onLinkClick} to="/management/list" className="nav-item pl-9 pb-3">
            Manage Management Files
          </Link>
          <Link
            onClick={onLinkClick}
            to="/management-activities/list"
            className="nav-item pl-9 pb-3"
          >
            Manage Management Activities
          </Link>
        </>
      )}
      {hasClaim(Claims.MANAGEMENT_ADD) && (
        <Link
          onClick={onLinkClick}
          to="/mapview/sidebar/management/new"
          className="nav-item pl-9 pb-3"
        >
          Create Management File
        </Link>
      )}
    </HalfHeightDiv>
  );
};
