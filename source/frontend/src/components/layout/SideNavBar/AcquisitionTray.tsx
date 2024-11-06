import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';

/**
 * Acquisitions page with links to acquisition functionality.
 * Intended for use within the left side tray.
 */
export const AcquisitionTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      {hasClaim(Claims.ACQUISITION_VIEW) && (
        <Link className="pl-9 pb-3 nav-item" onClick={onLinkClick} to="/acquisition/list">
          Manage Acquisition Files
        </Link>
      )}
      {hasClaim(Claims.ACQUISITION_ADD) && (
        <Link
          className="pl-9 pb-3 nav-item "
          onClick={onLinkClick}
          to="/mapview/sidebar/acquisition/new"
        >
          Create an Acquisition File
        </Link>
      )}
    </>
  );
};
