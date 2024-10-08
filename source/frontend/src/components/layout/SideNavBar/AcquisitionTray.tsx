import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Acquisitions page with links to acquisition functionality.
 * Intended for use within the left side tray.
 */
export const AcquisitionTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <Styled.TrayHeader>Acquisition Files</Styled.TrayHeader>
      {hasClaim(Claims.ACQUISITION_VIEW) && (
        <Link onClick={onLinkClick} to="/acquisition/list" className="nav-item">
          Manage Acquisition Files
        </Link>
      )}
      {hasClaim(Claims.ACQUISITION_ADD) && (
        <Link onClick={onLinkClick} to="/mapview/sidebar/acquisition/new" className="nav-item">
          Create an Acquisition File
        </Link>
      )}
    </>
  );
};
