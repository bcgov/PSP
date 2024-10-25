import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import { HalfHeightDiv } from './styles';

/**
 * Disposition Tray page.
 * Intended for use within the left side tray.
 */
export const DispositionTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <HalfHeightDiv>
        {hasClaim(Claims.DISPOSITION_VIEW) && (
          <Link onClick={onLinkClick} to="/disposition/list" className="nav-item pl-9 pb-3">
            Manage Disposition Files
          </Link>
        )}
        {hasClaim(Claims.DISPOSITION_ADD) && (
          <Link
            onClick={onLinkClick}
            to="/mapview/sidebar/disposition/new"
            className="nav-item pl-9 pb-3"
          >
            Create a Disposition File
          </Link>
        )}
      </HalfHeightDiv>
    </>
  );
};
